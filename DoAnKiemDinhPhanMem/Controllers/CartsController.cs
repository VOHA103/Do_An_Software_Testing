using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DoAnKiemDinhPhanMem.Models;

namespace DoAnKiemDinhPhanMem.Controllers
{
    public class CartsController : Controller
    {
        private cuasneakerEntities db = new cuasneakerEntities();

        private const string cartSession = "cartSession";

        // GET: Carts
        public ActionResult Index()
        {
            var cart = Session[cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult DeleteItem(int id)
        {
            var cart = Session[cartSession];
            Products p = db.Products.Find(id);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.Id == id))
                {
                    list.RemoveAll(r => r.product.Id == id);
                    //foreach (var item in list)
                    //{
                    //    if (item.Product.Id == ProductId)
                    //        list.Remove(item);
                    //}
                    Session[cartSession] = list;
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carts carts = db.Carts.Find(id);
            db.Carts.Remove(carts);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AddItem(int ProductId, int Quantity)
        {
            var cart = Session[cartSession];//bien Cart co ten la cartSession
            Products p = db.Products.Find(ProductId);
            //tim kiem san pham trong db, Id=1
            if (cart != null)
            {//gio da co sp
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.Id == ProductId))
                {
                    foreach (var item in list)
                    {
                        if (item.product.Id == ProductId)
                            item.Quantity += Quantity;
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.product = p;
                    item.Quantity = Quantity;
                    list.Add(item);
                    Session[cartSession] = list; //save
                }

            }
            else
            { //gio hang moi
                var item = new CartItem();
                item.product = p;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[cartSession] = list;//save 
            }

            return RedirectToAction("Index");
        }
        public ActionResult UpdateItem(int ProductId, int Quantity)
        {
            var cart = Session[cartSession];
            Products p = db.Products.Find(ProductId);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.Id == ProductId))
                {
                    if (Quantity <= 0)
                        list.RemoveAll(r => r.product.Id == ProductId);
                    else
                        foreach (var item in list)
                        {
                            if (item.product.Id == ProductId)
                                item.Quantity = Quantity;
                        }
                }
                Session[cartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public ActionResult UpdateSize(int ProductId, int Size)
        {
            var cart = Session[cartSession];
            Products p = db.Products.Find(ProductId);
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.product.Id == ProductId))
                {
                    if (Size <= 0)
                        list.RemoveAll(r => r.product.Id == ProductId);
                    else
                        foreach (var item in list)
                        {
                            if (item.product.Id == ProductId)
                                item.product.Size = Size;
                        }
                }
                Session[cartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public ActionResult NhapMail()
        {
            return View();
        }       
        [HttpPost]

        public JsonResult sendMail(string mail)
        {

            var obj = db.Accounts.Where(a => a.Email.Equals(mail)).FirstOrDefault();
            if (obj == null)
                return Json(new
                {
                    status = false,
                    message = "Email không tồn tại."
                });
            // //đăng nhập mail để gửi
            string email = ConfigurationManager.AppSettings["mail"].ToString();
            string pass = ConfigurationManager.AppSettings["pass"].ToString();

            //gán thông tin
            var mess = new MailMessage(email, mail);
            mess.Subject = string.Format("Thông tin đơn hàng của khách hàng {0}", obj.Fullname);
            string text = string.Empty;
            int i = 1;
            var list = (Session[cartSession] as List<CartItem>);

            text = String.Format(@"
            <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                <tr>
                  <td>
                    <table align='center' border='0' cellpadding='0' cellspacing='0' width='00' style='border-collapse: collapse;'>
                      <tr>
                        <td bgcolor='#70bbd9'>
                          <table border='1' cellpadding='0' cellspacing='0' width='100%' >
                            <tr>
                      
                              <td align='center' style='padding: 10px;'>
                              <h1>SHOP SHOE CUASNEAKER</h1>
                              </td>
                            </tr>
                            <tr>
                                <td align='center' style='padding: 10px;'>
                                <h3>THÔNG TIN KHÁCH HÀNG</h3>
                                </td>
                              </tr>
                            <tr>
                                <td>
                                    <table border='1' cellpadding='0' cellspacing='0' width='100%' >
                                      <tr>
                                        <td width='50%' valign='top' align='center' style='padding: 30px;'>
                                        <h4>Họ tên : {0}</h4>
                                        <h4>Số Điện Thoại : {1}</h4>
                                        </td>
                                        <td width='50%' valign='top' align='center' style='padding: 30px'>
                                            <h4>Email : {2}</h4>
                                            <h4>Địa Chỉ : {3}</h4>
                                        </td>
                                      </tr>
                                    </table>
                                  </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>", obj.Fullname, obj.Phone, obj.Email, obj.Address);
            text += @"
                <table align='center' border='1' cellpadding='1' cellspacing='5' style='width:1000px'>
	                <thead>
		                <tr>
			                <th scope='row'>STT</th>
			                <th scope='col'>T&ecirc;n sản phẩm</th>
			                <th scope='col'>Gi&aacute;</th>
			                <th scope='col'>Số lượng</th>
			                <th scope='col'>Size</th>
			                <th scope='col'>Th&agrave;nh tiền</th>
		                </tr>
	                </thead>
	                <tbody>";
            foreach (var item in list)
            {
                Carts cart = new Carts();
                cart.AccountId = obj.Id;
                cart.ProductId = item.product.Id;
                cart.Quantity = item.Quantity;
                db.Carts.Add(cart);
                db.SaveChanges();
                text += String.Format(@"<tr><th scope='row'>{0}</th><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                i, item.product.Productname, item.product.Price, item.Quantity, item.product.Size, item.Quantity * item.product.Price);
                i++;
            }
            text += String.Format(@"<tr>
                                    <th colspan='2'><p align='center'>Tổng tiền : </p></th>
                                    <th colspan='3'><p align='center'> {0} </p></th>
                                    </tr>
                                </tbody>
                            </table>", list.Sum(x => x.Quantity * x.product.Price));
            mess.Body = text;
            //cho gửi định dạng html
            mess.IsBodyHtml = true;
            //cấu hình mail
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            //gửi mail đi
            NetworkCredential net = new NetworkCredential(email, pass);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = net;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                smtp.Send(mess);
            }
            catch (SmtpException ex)
            {
                return Json(new
                {
                    status = false,
                    message="Có lỗi gởi mail xảy ra."
                });
            }
            return Json(new
            {
                status = true,
                url = "Index",
                message = "Gửi mail thành công."
            });
        }
    }
}
