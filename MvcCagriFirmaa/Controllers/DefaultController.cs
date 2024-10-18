using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcCagriFirmaa.Models.Entity;

namespace MvcCagriFirmaa.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        DbisTakipEntities db = new DbisTakipEntities();



        public ActionResult AktifCagrilar()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault(); // sisteme hangi mail ile girdiysek onun id numarasını alıyoruz
            var cagrilar = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).ToList(); // daha sonra burda da o ıd ye göre değerleri getiriyoruz



            return View(cagrilar);
        }

        public ActionResult PasifCagrilar()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false && x.CagriFirma == id).ToList();

            return View(cagrilar);
        }


        [HttpGet] // sayfam yüklendiği zaman ne olsun 
        public ActionResult YeniCagri()
        {

            return View();
        }
        [HttpPost]

        public ActionResult YeniCagri(TblCagrilar p)
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");

        }

        public ActionResult CagriDetay(int id)
        {
            // tbl çağrılardaki çağrı firma numarası  tblçağrıdetaydaki cagri ya eşit olmalı
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();


            return View(cagri);
        }

        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir", cagri);
        }

        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.ID);
            cagri.Konu = p.Konu;  // cağrılarda bulunan konuyu p parametresinden aldığım konu değeri ile değiştir
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        [HttpGet]
       


        public ActionResult ProfilGüncelle()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault(); // sisteme hangi mail ile girdiysek onun id numarasını alıyoruz
            var Bilgiler = db.TblFirmalar.Where(x => x.ID==id ).ToList(); 



            return View(Bilgiler);
        }

        public ActionResult ProfilGetir(int id)
        {
            var profil = db.TblFirmalar.Find(id);
            return View("ProfilGetir", profil);
        }
        public ActionResult ProfilGüncelleson(TblFirmalar p)
        {
            var deger = db.TblFirmalar.Find(p.ID);

            deger.Ad = p.Ad;
            deger.Yetkili = p.Yetkili;
            deger.Telefon = p.Telefon;
            deger.Mail = p.Mail;
            deger.Sifre = p.Sifre;
            deger.Sektor = p.Sektor;
            deger.il = p.il;
            deger.ilce = p.ilce;
            deger.Adres = p.Ad;
            db.SaveChanges();
            return RedirectToAction("ProfilGüncelle");
        }



        public ActionResult AnaSayfa()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var toplamcagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            var aktifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            var pasifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            var yetkili = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            var sektör = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Sektor).FirstOrDefault();
            var firmaadi = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Ad).FirstOrDefault();
            var firmagörsel = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Görsel).FirstOrDefault();

           

            ViewBag.c1 = toplamcagri;
            ViewBag.c2 = aktifcagri;
            ViewBag.c3 = pasifcagri;
            ViewBag.c4 = yetkili;
            ViewBag.c5 = sektör;
            ViewBag.c6 = firmaadi;
            ViewBag.c7 = firmagörsel;

            return View();
        }

        public PartialViewResult partial1() //  belirli bır kısmı bölmek için kullanıyoruz mesela burda mesajlar kısmı için kullandık
        {
            // true okunmamış mesaj - false okunmuş mesaj 
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var mesajlar = db.TblMesajlar.Where(x => x.Alıcı == id && x.Durum == true).ToList(); // buraya bakıcam
            var mesajsay = db.TblMesajlar.Where(x => x.Alıcı == id && x.Durum == true).Count();

            ViewBag.m1 = mesajsay;

            return PartialView(mesajlar);
        }

        public PartialViewResult partial2()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).ToList();
            var cagrisay = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.m1 = cagrisay;

            return PartialView(cagrilar);
        }

        

        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // oturum sonlandırma
            return RedirectToAction("Index", "Login");
        }

        public PartialViewResult Partial3()
        {
         /*   var deneme = db.TblMesajlar.Where(x => x.ID == 1).Select(y => y.Mesaj).FirstOrDefault();

            ViewBag.deneme = deneme; bu kısım  partial3 te yer alıyor denedik çalıştı öğrenmış olduk. */

            return PartialView();
            
        }



        public ActionResult Gelenmesajlar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var listele =  db.TblMesajlar.Where(x=>x.Alıcı==id).ToList();
           

            return View(listele);
        }

        [HttpGet]
        public ActionResult YeniMesaj()
        {
            return View();
        }

        [HttpPost]

       public ActionResult YeniMesaj(TblMesajlar t)
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();


            t.Gonderen = id;
            t.Durum = true;
            db.TblMesajlar.Add(t);
            db.SaveChanges();
            return RedirectToAction("Gelenmesajlar");
        }


        public ActionResult GönderilenMesajlar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();

            var listele = db.TblMesajlar.Where(x => x.Gonderen == id).ToList();
            return View(listele);
        }


    }
}