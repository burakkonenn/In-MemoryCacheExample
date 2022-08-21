using In_MemoryCacheExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace In_MemoryCacheExample.Controllers
{
    public class HomeController : Controller
    {

        IMemoryCache _memoryCache;
        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = _memoryCache;
        }

        public IActionResult SetCaches()
        {

            ///SetCaches metodu, PersonalName(key) ve BurakOnen(value) değerlerinden
            ///oluşan datayı memory'e cacheleyecek olan metot.



            _memoryCache.Set("PersonalName", "BurakOnen");

            return View();
        }

        public IActionResult GetCaches()
        {

            //GetCache metodu ise, PersonalName(key) değer ile set edilen cachede ki datayı
            ///okumak/elde etmek için kullanılır


            var cache = _memoryCache.Get<string>("PersonalName");
            return View();
        }

        public IActionResult TryGetValues()
        {

           ///TryGetValues metodu cachede belirtilen key(PersonalName) değerine göre
           ///uygun verinin olup olmadığını sorgular.Eğer yoksa false var ise true döndürerek 2. parametresinde ki out ile cacheden datayı döndürür.

            if (_memoryCache.TryGetValue<string>("PersonalName", out string data))
            {
                //data burada elde edilir.
            }
            return View();
        }


        public IActionResult GetOrCreate()
        {

            //GetOrCreate metodu, belirtilen key değerine göre data var mı kontrol eder.
            ///Eğer yoksa datayı oluşturur.


            var getOrCreateCache = _memoryCache.GetOrCreate<string>("PersonalName", data =>
            {
                data.SetValue("BurakOnen");
                Console.WriteLine(DateTime.Now);
                return data.Value.ToString();
            });

            return View();
        }


        public void AbsoluteExpirationSlidingExpiration()
        {

            ///AbsoluteExpirationSlidingExpiration metodu, cachede tutulacak dataların yaşam sürecini belirtir.
            //AbsoluteExpiration, belirttiğimiz süre boyunca datanın cachede duracağını belirtir.Süre sonunda data cacheden silinir.
            ///SlidenExpiration, belirttiğimiz süre içerisinde cachede ki dataya bir istek geldiği taktirde süre bir o kadar daha uzayacaktır.Eğer talep gelmezse data silinecektir.





            DateTime date = _memoryCache.GetOrCreate<DateTime>("date", entry =>
            {
                entry.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
                entry.SlidingExpiration = TimeSpan.FromSeconds(5);
                DateTime value = DateTime.Now;
                Console.WriteLine($"*** Set Cache : {value}");
                return value;
            });

            Console.WriteLine($"Get Cache : {date}");
        }


        public void Priority()
        {


            ///yayın süresi boyunca cachede tutulacak olan veriler memory'i şişirebilir.Ve yeni veriler
            ///var olan veriler sistem tarafından silinmek istenebilir. İşte böyle bi durumda cache'den silinecek olan verilerin önceliklerini ve hangi durumda kalacaklarını belirtebiliriz.
            ///LOW: Önem derecesi en düşük olan,
            ///NORMAL:Önem derecesi lowdan sonra gelen datalar,
            ///HİGH:Önemli veridir

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.Priority = CacheItemPriority.High;
            _memoryCache.Set("date", DateTime.Now, options);
        }





        public IActionResult SetCache()
        {
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(20);
            options.SlidingExpiration = TimeSpan.FromSeconds(5);
            _memoryCache.Set("date", DateTime.Now, options);

            return RedirectToAction(nameof(GetCache));


        }


        public IActionResult GetCache()
        {
            if (_memoryCache.TryGetValue<DateTime>("date", out DateTime date))
            {
                Console.WriteLine($"Get Cache : {date}");
            }
            return View();
        }



    }
}