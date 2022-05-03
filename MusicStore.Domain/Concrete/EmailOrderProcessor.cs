using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using MusicStore.Domain.Entities;
using MusicStore.Domain.Abstract;
using System.Data.Entity;

namespace MusicStore.Domain.Concrete
{
    //konfiguracja adresu email wysyłkowego
    public class EmailSettings
    {
        public string MailToAddress = "zamowienia@przyklad.pl";
        public string MailFromAddress = "musicstore@przyklad.pl";
        public bool UseSsl = true;
        public string Username = "UżytkownikSmtp";
        public string Password = "HasłoSmtp";
        public string ServerName = "smtp.przyklad.pl";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\Users\pawla\source\repos\MusicStore\Mail";

    }

    //implementacja interfejsu oraz wysyłania maili 
    public class EFOrdersRepository : IOrderProcessor
    {
        private EfDbContext context = new EfDbContext();
        private EmailSettings emailSettings;

        public EFOrdersRepository(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using(var smtpClinet = new SmtpClient())
            {
                smtpClinet.EnableSsl = emailSettings.UseSsl;
                smtpClinet.Host = emailSettings.ServerName;
                smtpClinet.Port = emailSettings.ServerPort;
                smtpClinet.UseDefaultCredentials = false;
                smtpClinet.Credentials= new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClinet.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClinet.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClinet.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder().AppendLine("Nowe zamówienie").AppendLine("---").AppendLine("Produkty");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.album.Price;
                    body.AppendFormat(line.Product.album.Name, subtotal);
                }
                body.AppendFormat("Wartośc całkowita: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Wysyłka dla:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---");

                MailMessage mailMessage = new MailMessage
                (
                    emailSettings.MailFromAddress,//od 
                    emailSettings.MailToAddress, //do
                    "Otrzymano nowe zamówienie!",//temat
                    body.ToString() //tresc
                );

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClinet.Send(mailMessage);
            }
        }

        public async Task<List<Order>> AllOrdersAsync() => await context.Order.ToListAsync().ConfigureAwait(false);
        public async Task<List<OrderAlbum>> AllOrdersAlbumAsync() => await context.OrdersAlbums.ToListAsync().ConfigureAwait(false);

        public async Task NewOrder(int clientId, List<int> albumsId, decimal price)
        {
            //pobranie daty systemowej
            DateTime myDateTime = DateTime.Now;
            var nameOrder = GetLast() + "/" + DateTime.Now.ToString("yyyy");
            //utworzenie nowego zamówienia
            Order order = new Order()
            {
                ClientId = clientId,
                Data=myDateTime,
                Name = nameOrder,
                Price = price,
            };
            //dodanie do bazy 
            context.Order.Add(order);
            await context.SaveChangesAsync();

            //pobranie id zamówienia
            var AllOrders = await AllOrdersAsync();
            Order x = AllOrders.Where(y => y.Data == myDateTime).FirstOrDefault();
            int id = x.Id;

            //zapisywanie id albumów do id zamówienia
            foreach (var albumId in albumsId)
            {
                OrderAlbum nowy = new OrderAlbum()
                {
                    AlbumId = albumId,
                    OrderId = id,
                };

                context.OrdersAlbums.Add(nowy);
            }
            //zapisanie danych w bazie
            await context.SaveChangesAsync();
        }

        public int GetLast()
        {
            var a = context.Order.ToList();
            if (a.Count > 0)
            {
                var AllOrders = int.Parse(context.Order
                                .OrderByDescending(p => p.Id)
                                .Select(r => r.Id)
                                .First().ToString());
                return AllOrders;
            }
            else
                return 1;
        }

        public async Task<decimal> GetAllMoneyEarned()
        {
            decimal a = 0;
            var list = await AllOrdersAsync();
            foreach (var item in list)
            {
                a += item.Price;
            }
            return a;
        }
       
    }
}
