using BookStoreServer.Context;
using BookStoreServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace BookStoreServer.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ConfigurationsController : ControllerBase
{
    BookwormDbContext _context = new BookwormDbContext();


    [HttpGet]
    public IActionResult AddAUser()
    {
        var user1 = new User
        {
            Name = "Ali",
            LastName = "Yılmaz",
            DisplayName = "ali_34",
            Email = "ali@gmail.com",
            Password = "123456",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "ali@gmail.com",
            ContactName = "Ali Yılmaz",
            ContactPhoneNumber = "05551234567",
            City = "İSTANBUL",
            Country = "Turkey",
            ZipCode ="34000",
            AddressDescription = "Kadıköy Mah. Aydın Apt. No: 5/2 Kadıköy/İstanbul",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "ali@gmail.com",
            ContactName = "Ali Yılmaz",
            ContactPhoneNumber = "05551234567",
            City = "İSTANBUL",
            Country = "Turkey",
            ZipCode ="34000",
            AddressDescription = "Kadıköy Mah. Aydın Apt. No: 5/2 Kadıköy/İstanbul",
        }
    }
        };

        var user2 = new User
        {
            Name = "Ayşe",
            LastName = "Kaya",
            DisplayName = "ayse_28",
            Email = "ayse@gmail.com",
            Password = "abcdef",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "ayse@gmail.com",
            ContactName = "Ayşe Kaya",
            ContactPhoneNumber = "05557654321",
            City = "ANKARA",
            Country = "Turkey",
            ZipCode ="06000",
            AddressDescription = "Çankaya Mah. Gül Sok. No: 10/3 Çankaya/Ankara",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "ayse@gmail.com",
            ContactName = "Ayşe Kaya",
            ContactPhoneNumber = "05557654321",
            City = "ANKARA",
            Country = "Turkey",
            ZipCode ="06000",
            AddressDescription = "Çankaya Mah. Gül Sok. No: 10/3 Çankaya/Ankara",
        }
    }
        };

        var user3 = new User
        {
            Name = "Mehmet",
            LastName = "Kurtuluş",
            DisplayName = "mehmet_45",
            Email = "mehmet@gmail.com",
            Password = "789012",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "mehmet@gmail.com",
            ContactName = "Mehmet Kurtuluş",
            ContactPhoneNumber = "05559876543",
            City = "İZMİR",
            Country = "Turkey",
            ZipCode ="35000",
            AddressDescription = "Konak Mah. Atatürk Cad. No: 15/4 Konak/İzmir",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "mehmet@gmail.com",
            ContactName = "Mehmet Kurtuluş",
            ContactPhoneNumber = "05559876543",
            City = "İZMİR",
            Country = "Turkey",
            ZipCode ="35000",
            AddressDescription = "Konak Mah. Atatürk Cad. No: 15/4 Konak/İzmir",
        }
    }
        };

        var user4 = new User
        {
            Name = "Fatma",
            LastName = "Şahin",
            DisplayName = "fatma_23",
            Email = "fatma@gmail.com",
            Password = "xyz789",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "fatma@gmail.com",
            ContactName = "Fatma Şahin",
            ContactPhoneNumber = "05551234567",
            City = "ISTANBUL",
            Country = "Turkey",
            ZipCode ="34000",
            AddressDescription = "Levent Mah. Cemal Sk. No: 7/3 Levent/ISTANBUL",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "fatma@gmail.com",
            ContactName = "Fatma Şahin",
            ContactPhoneNumber = "05551234567",
            City = "ISTANBUL",
            Country = "Turkey",
            ZipCode ="34000",
            AddressDescription = "Levent Mah. Cemal Sk. No: 7/3 Levent/ISTANBUL",
        }
    }
        };

        var user5 = new User
        {
            Name = "Mustafa",
            LastName = "Yılmaz",
            DisplayName = "mustafa_56",
            Email = "mustafa@gmail.com",
            Password = "abc123",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "mustafa@gmail.com",
            ContactName = "Mustafa Yılmaz",
            ContactPhoneNumber = "05557654321",
            City = "ANKARA",
            Country = "Turkey",
            ZipCode ="06000",
            AddressDescription = "Kızılay Mah. Atatürk Cad. No: 12/5 Kızılay/ANKARA",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "mustafa@gmail.com",
            ContactName = "Mustafa Yılmaz",
            ContactPhoneNumber = "05557654321",
            City = "ANKARA",
            Country = "Turkey",
            ZipCode ="06000",
            AddressDescription = "Kızılay Mah. Atatürk Cad. No: 12/5 Kızılay/ANKARA",
        }
    }
        };

        var user6 = new User
        {
            Name = "Zeynep",
            LastName = "Güneş",
            DisplayName = "zeynep_78",
            Email = "zeynep@gmail.com",
            Password = "def456",

            Addresses = new List<Address>
    {
        new Address
        {
            AddressType = "Billing",
            ContactEmail = "zeynep@gmail.com",
            ContactName = "Zeynep Güneş",
            ContactPhoneNumber = "05559876543",
            City = "IZMIR",
            Country = "Turkey",
            ZipCode ="35000",
            AddressDescription = "Alsancak Mah. İnönü Cad. No: 20/2 Alsancak/IZMIR",
        },
        new Address
        {
            AddressType = "Shipping",
            ContactEmail = "zeynep@gmail.com",
            ContactName = "Zeynep Güneş",
            ContactPhoneNumber = "05559876543",
            City = "IZMIR",
            Country = "Turkey",
            ZipCode ="35000",
            AddressDescription = "Alsancak Mah. İnönü Cad. No: 20/2 Alsancak/IZMIR",
        }
    }
        };


        _context.Users.AddRange(user1, user2, user3, user4, user5, user6);
        _context.SaveChanges();

        return Ok(_context.Users.ToList());
    }

    [HttpGet]
    public IActionResult AddAuthor()
    {
        var author = new Author
        {
            FullName = @"Rachel Abbott",
            BiographyEn = @"Rachel Abbott is a British author and successful creator of bestselling psychological thriller novels. Abbott began his previous career as an executive in business and technology, but focused on writing after his retirement. His first novel, ""Only the Innocent"" (2011), was a huge international success and made Abbott a household name in the literary world. His works, which stand out with character development, gripping stories and unexpected twists, draw their readers into the depths of the thriller genre. By combining her passion for writing with her dedication to exploring complex relationships and the darker aspects of human nature, Abbott has forged her own brand in the psychological thriller genre.",
            BiographyTr = @"Rachel Abbott, İngiliz yazar ve çok satan psikolojik gerilim romanlarının başarılı bir yaratıcısıdır. Abbott, önceki kariyerine ticaret ve teknoloji alanında yöneticilik yaparak başlamış, ancak emekliliğinden sonra yazmaya odaklanmıştır. İlk romanı olan ""Only the Innocent"" (2011), uluslararası alanda büyük bir başarı elde etti ve Abbott'u edebi dünyada tanınan bir isim haline getirdi. Karakter gelişimi, sürükleyici hikayeler ve beklenmedik twist'lerle dikkat çeken eserleri, okuyucularını gerilim türünün derinliklerine çekiyor. Abbott, yazma tutkusunu, karmaşık ilişkileri ve insan doğasının karanlık yönlerini keşfetmeye olan bağlılığıyla birleştirerek, psikolojik gerilim türünde kendi markasını oluşturdu.",
            ProfilePhotoImgUrl = "https://m.media-amazon.com/images/S/amzn-author-media-prod/fv4v7cjf0766c96baua29u1ede._SY1000_CR0%2C0%2C1000%2C1000_.jpg"
        };
        _context.Authors.Add(author);
        _context.SaveChanges();
        return Ok(_context.Authors.ToList());
    }



    [HttpGet]
    public async Task<IActionResult> AddSampleBook()
    {

        // Kullanıcı sayısını alın
        var userIds = _context.Users.Select(u=>u.Id).ToList();

        var book = new Book
        {
            AuthorId = _context.Authors.Where(p=>p.FullName == "İlber Ortaylı").FirstOrDefault().Id,
            TitleTr = "Gazi Mustafa Kemal Atatürk",
            TitleEn = "Mustafa Kemal Atatürk, the Victorious",
            SubTitleTr = @"""Tarihin akışını değiştiren, ona mührünü vuran veya büyük tehlikelere mani olan liderlere her memlekette rastlamak mümkün değildir. Atatürk dünya tarihinin nadiren gördüğü bir dehadır. Birinci Dünya Savaşı'ndan sonra, hiçbir mağlup milletin direniş göstermediği zamanda siviller ve askerlerle dünyaya meydan okumuştur.""
              İLBER ORTAYLI",
            SubTitleEn = @"The leaders who change the course of history, leave their mark on it, or prevent great dangers are not encountered in every country. Atatürk is a rare genius in world history. After the First World War, at a time when no defeated nation resisted, he defied the world with civilians and soldiers."" - İlber Ortaylı",
            MainImgUrl = "https://m.media-amazon.com/images/I/51jUzMOHwkL._SL1000_.jpg",
            ISBN = "9752430295",
            DescriptionEn = @"On the back cover:
All aspects of the life of the great leader Atatürk... 'It is not possible to encounter leaders in every country who change the course of history, leave their mark on it, or prevent great dangers. Atatürk is a rare genius in world history. After the First World War, at a time when no defeated nation resisted, he defied the world with civilians and soldiers.' - İlber Ortaylı

The book 'Gazi Mustafa Kemal Atatürk,' begins with the generation of the 1880s, the first to revive the empire, the Balkan geography, and Mustafa Kemal's family background. It then continues with Atatürk's military education, years in Manastır, the Period of Nationalism, the Committee of Union and Progress, Sultan Abdulhamid II, Enver Pasha, Ziya Gökalp, Trablusgarb, the Balkan Wars, and the years in Sofia. The First World War, in which our army fought on eight fronts against the Allied powers, the glorious victories of Çanakkale and Kutü’l Amâre, the Armistice of Mudros, the last Ottoman Sultan Vahideddin, and the death sentence for a nation and a country called Sevres... The entire period of the War of Independence in detail, the opposition against the Independence War given despite, the Battles of İnönü, the Lausanne Conference, the Great Offensive, and the path to the Republic... Debates on the sultanate and caliphate, Lausanne, the Twelve Islands, population exchange, debts left by the Ottoman Empire, Mosul, and the most important issues of recent history, revolutions... Lastly, with his personal qualities, the founder of modern Turkey, Atatürk, whose traces remain in the world, in memories, and in minds... In this first biography, İlber Ortaylı narrates the life of the great leader Mustafa Kemal Atatürk in all its aspects. He adds another guidebook to Turkish historiography that will never be forgotten and will be constantly referred to...",


            DescriptionTr = @"Arka Kapaktan
Yaşamının tüm yönleriyle büyük lider Atatürk… “Tarihin akışını değiştiren, ona mührünü vuran veya büyük tehlikelere mani olan liderlere her memlekette rastlamak mümkün değildir. Atatürk dünya tarihinin nadiren gördüğü bir dehadır. Birinci Dünya Savaşı'ndan sonra, hiçbir mağlup milletin direniş göstermediği zamanda siviller ve askerlerle dünyaya meydan okumuştur.” - İlber Ortaylı Gazi Mustafa Kemal Atatürk kitabı, evvela imparatorluğu dirilten nesil olan 1880'liler kuşağı, Balkan coğrafyası ve Mustafa Kemal'in aile kökeni ile başlıyor. Akabinde Atatürk’ün askeri eğitimi, Manastır yılları, Milliyetçilikler Dönemi, İttihat ve Terakki, 2. Abdülhamid, Enver Paşa, Ziya Gökalp, Trablusgarb, Balkan Savaşları ve Sofya yıllarıyla devam ediyor. Ordumuzun İtilaf devletleriyle sekiz cephede mücadele ettiği Birinci Dünya Savaşı, kutlu zaferlerimiz Çanakkale ve Kutü’l Amâre, Mondros, son padişah Vahideddin, bir milletin ve ülkenin ölüm fermanı olan Sevr… Tüm detaylarıyla Milli Mücadele dönemi, 23 Nisan 1920 ve sonrasında muhalefete rağmen verilen Kurtuluş Savaşı, İnönü Muharebeleri, Lozan Konferansı, Büyük Taarruz ve Cumhuriyet’e giden yol... Saltanat ve hilafet tartışmaları, Lozan, On İki Ada, mübadele, Osmanlı'dan kalan borçlar, Musul ve yakın tarihin en önemli meselesi olan inkılablar... Son olarak kişisel özellikleriyle, dünyada, anılarda, hafızalarda kalan izleriyle modern Türkiye’nin kurucusu Atatürk... İlber Ortaylı bu ilk biyografisinde yaşamının tüm yönleriyle büyük lider Gazi Mustafa Kemal Atatürk'ü anlatıyor. Türk tarihçiliğine hiç unutulmayacak ve sürekli başvurulacak bir rehber kitap daha kazandırıyor...",

            

            BookVariations = new List<BookVariation>
                    {
                        new BookVariation
                        {
                            FormatEn = Enums.BookFormatEnumEn.Hardcover,
                            FormatTr = Enums.BookFormatEnumTr.Ciltli,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceAmount = 84.99m,
                                    PriceCurrency = "₺"
                                }
                            },
                            ProductDetail = new ProductDetail
                            {
                                FormatTr = "480 sayfa",
                                FormatEn = "480 pages",
                                Dimensions = "13.4 x 3.2 x 21.1 cm",
                                PublicationDate = new DateTime(2018, 01, 11),
                                PublisherTr = "Kronik Kitap; 1. basım",
                                PublisherEn = "Chronicle Book; 1st edition",
                                LanguageTr = "Türkçe",
                                LanguageEn = "Turkish"
                            },

                        },
                        new BookVariation
                        {
                            FormatEn = Enums.BookFormatEnumEn.Audiobook,
                            FormatTr = Enums.BookFormatEnumTr.SesliKitap,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceAmount = 59.99m,
                                    DiscountedPriceAmount = 49.99m,
                                    PriceCurrency = "₺"
                                }
                            },
                            ProductDetail = new ProductDetail
                            {
                                FormatTr = "3 saat 45 dakika",
                                FormatEn = "3 hours and 45 minutes",
                                Dimensions = "13.4 x 3.2 x 21.1 cm",
                                PublicationDate = new DateTime(2018, 01, 11),
                                PublisherTr = "Kronik Kitap; 1. basım",
                                PublisherEn = "Chronicle Book; 1st edition",
                                LanguageTr = "Türkçe",
                                LanguageEn = "Turkish"
                            },

                        },
                        new BookVariation
                        {
                            FormatEn = Enums.BookFormatEnumEn.Kindle,
                            FormatTr = Enums.BookFormatEnumTr.EKitap,
                            Prices = new List<Price>
                            {
                                new Price
                                {
                                    PriceAmount = 39.99m,
                                    //DiscountedPriceAmount = 44.99m,
                                    PriceCurrency = "₺"
                                }
                            },
                            ProductDetail = new ProductDetail
                            {
                                FormatTr = "604 KB",
                                FormatEn = "604 KB",
                                Dimensions = "13.4 x 3.2 x 21.1 cm",
                                PublicationDate = new DateTime(2018, 01, 11),
                                PublisherTr = "Kronik Kitap; 1. basım",
                                PublisherEn = "Chronicle Book; 1st edition",
                                LanguageTr = "Türkçe",
                                LanguageEn = "Turkish"
                            },

                        },
                    },
            BookImgUrls = new List<BookImgUrl>
                    {
                //mainImgUrl
                        new BookImgUrl
                        {
                            ImgUrl = "https://m.media-amazon.com/images/I/51jUzMOHwkL._SL1000_.jpg"
                        },
                //mainImgUrl
                        new BookImgUrl
                        {
                            ImgUrl = "https://www.kulturatek.com/shop/12556-thickbox_default/gazi-mustafa-kemal-ataturk-ilber-ortayli.jpg"
                        },
                        new BookImgUrl
                        {
                            ImgUrl = "https://www.kulturatek.com/shop/12557-thickbox_default/gazi-mustafa-kemal-ataturk-ilber-ortayli.jpg"
                        },
                        //new BookImgUrl
                        //{
                        //    ImgUrl = "https://m.media-amazon.com/images/I/61xx7rHwdTL._SL1200_.jpg"
                        //}
                    },
            BookVideoUrls = new List<BookVideoUrl>
                    {
                new BookVideoUrl
                {
                    CoverImgUrl = "https://i.ytimg.com/vi/G5zEOSwuY24/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLB9zCr-MN_RyGdZEUNoEy599Hri-w",
                    VideoUrl="https://www.youtube.com/embed/G5zEOSwuY24?si=MiFuEksRy7LzCZq-",
                },
                //new BookVideoUrl
                //{
                //    CoverImgUrl ="https://i.ytimg.com/vi/g8wkulcRPTY/hq720.jpg?sqp=-oaymwEcCNAFEJQDSFXyq4qpAw4IARUAAIhCGAFwAcABBg==&rs=AOn4CLDpr_ZNJLhzb-mneGlEmC6-SQimnQ",
                //    VideoUrl = "https://www.youtube.com/embed/g8wkulcRPTY?si=nnnW_BGPURsuttOM",
                //},

            },
            BookReviews = new List<BookReview>
                    {
                        new BookReview
                        {
                            UserId = userIds.OrderBy(u=> Guid.NewGuid()).FirstOrDefault(),
                            CommentTitleEn = "Seamless",
                            CommentTitleTr = "Sorunsuz",
                            CommentEn = @"Solid and fast shipping",


                            CommentTr = @"Sağlam ve hızlı kargo",
                            Raiting = 5,
                            CreatedAt = DateTime.Now,
                        },
                        new BookReview
                        {
                            UserId =userIds.OrderBy(u=> Guid.NewGuid()).FirstOrDefault(),
                            CommentTitleEn = "definitely get it",
                            CommentTitleTr = "kesinlikle alın",
                            CommentEn = @"Appreciate İlber Ortaylı while he is alive...",


                            CommentTr = @"İlber Ortaylı hayatta iken kıymetini bilin...",
                            Raiting = 5,
                            CreatedAt = DateTime.Now,
                        },
                        new BookReview
                        {
                            UserId = userIds.OrderBy(u=> Guid.NewGuid()).FirstOrDefault(),
                            CommentTitleEn = "I hope it is useful.",
                            CommentTitleTr = "Faydalı olmasını dilerim.",
                            CommentEn = @"Benefiting from İlber Ortaylı's life experience will be beneficial for people of all ages and situations. With its worldview, knowledge and sincerity, it offers its readers and followers content to spend quality time. On the occasion of this book, I wish İlber Hacam health and well-being.",


                            CommentTr = @"İlber Ortaylı'nın hayat tecrübesinden yararlanmak her yaştan ve durumdan insanlar için faydalı olacaktır. Dünya görüşü, bilgisi ve samimiyetiyle okurlarını ve takipçilerine kaliteli vakit geçerecek içerikler sunuyor. Bu kitap vesilesiyle İlber Hacama sağlık ve afiyet diliyorum.",
                            Raiting = 5,
                            CreatedAt = DateTime.Now,
                        },
//                        new BookReview
//                        {
//                            UserId = randomUser.Id,
//                            CommentTitleEn = "Awsome",
//                            CommentTitleTr = "Süper",
//                            CommentEn = @"This 2nd book about Tom ,Emma and Rasta was worth reading a conclusion to A terrible story of a child's life after her mother dies .
//Really enjoying this series",


//                            CommentTr = @"Tom, Emma ve Rasta hakkındaki bu 2. kitap, Annesi öldükten sonra bir çocuğun hayatının korkunç hikayesinin bir sonucu olarak okunmaya değerdi.
//Bu seriden gerçekten keyif alıyorum",
//                            Raiting = 5,
//                            CreatedAt = DateTime.Now,
//                        }
                    },
            

        };

        _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var bookCategories = new List<BookCategory>
        {
            new BookCategory
            {
                BookId = book.Id,
                CategoryId = 8
            },
            new BookCategory
            {
                BookId = book.Id,
                CategoryId = 1
            }
        };


        _context.BooksCategories.AddRangeAsync(bookCategories);


        //var bookCategory = new BookCategory
        //{
            
        //        BookId = book.Id,
        //        CategoryId = 1
            
        //};

        //_context.BooksCategories.Add(bookCategory);


        await _context.SaveChangesAsync();

        var books = await _context.Books.ToListAsync();

        return Ok(books);
    }


    [HttpGet]
    public IActionResult GetBooksCategories() 
    {
        return Ok(_context.BooksCategories.ToList());
    }
}
