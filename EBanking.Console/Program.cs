using ConsoleTableExt;
using EBanking.Console.DataAccessLayer;
using EBanking.Console.Model;
using System.Text;
using System.Text.RegularExpressions;

// Zbog cirilice
Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;
// Umesto VARCHAR smo u DDL-u za Ime i prezime stavili NVARCHAR da bi sql server mogao da cuva cirilicne karaktere

while (true)
{
    try
    {
        ShowMainMenu();
        var mainOption = Console.ReadLine();

        switch (mainOption)
        {
            case "0":
                {
                    Console.Clear();
                    Console.WriteLine("- Крај рада -");
                    return;
                }
            case "1":
                {
                    await UserUseCases();
                    break;
                }
            default:
                {
                    Console.WriteLine("Непозната опција. Покушајте поново..(притисните било који тастер за наставак)");
                    Console.ReadKey();
                    break;
                }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Грешка: {ex.Message} {Environment.NewLine} (притисните било који тастер за наставак)");
        Console.ReadKey();
    }
}

void ValidateUserData(User newUser)
{
    ValidateFirstName(newUser);
    //TODO: napisati ostale validacije...
}

void ValidateFirstName(User newUser)
{
    if (string.IsNullOrWhiteSpace(newUser.FirstName))
        throw new Exception("Морате унети име корисника");
    
    if (newUser.FirstName.Length > 100)
        throw new Exception("Име корисника не сме имати више од 100 карактера");
    
    if (newUser.FirstName.Length < 2)
        throw new Exception("Име корисника мора садржати бар два карактера");
    
    var regex = new Regex(@"^[АБВГДЂЕЖЗИЈКЛЉМНЊОПРСТЋУФХЦЧЏШ][абвгдђежзијклљмнњопрстћуфхцчџш]+([ -]{0,1}[АБВГДЂЕЖЗИЈКЛЉМНЊОПРСТЋУФХЦЧЏШ][абвгдђежзијклљмнњопрстћуфхцчџш]+)*$");

    if (regex.IsMatch(newUser.FirstName) == false)
        throw new Exception("Име корисника мора бити написано ћириличним писмом и прво слово мора бити велико");
}

void ShowMainMenu()
{
    Console.Clear();
    Console.WriteLine("1. Корисници");
    Console.WriteLine("2. Рачуни");
    Console.WriteLine("3. Трансакције");
    Console.WriteLine("4. Валуте");
    Console.WriteLine("0. Крај");
    Console.Write("Одаберите опцију: ");
}


async Task UserUseCases()
{
    var goBackRequested = false;
    while (goBackRequested == false)
    {
        try
        {
            ShowUserMenu();
            var userOption = Console.ReadLine();
            Console.Clear();
            switch (userOption)
            {
                case "0":
                    {
                        goBackRequested = true;
                        break;
                    }
                case "1":
                    {
                        Console.WriteLine("Унесите име:");
                        var firstName = Console.ReadLine() ?? "";

                        Console.WriteLine("Унесите презиме:");
                        var lastName = Console.ReadLine() ?? "";

                        Console.WriteLine("Унесите корисничку адресу:");
                        var email = Console.ReadLine() ?? "";

                        Console.WriteLine("Унесите шифру:");
                        var password = Console.ReadLine() ?? "";

                        var newUser = new User()
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email,
                            Password = password
                        };

                        ValidateUserData(newUser);

                        var user = await SqlRepository.CreateUser(newUser);

                        Console.WriteLine($"Додат нови корисник: '{user}'. (притисните било који тастер за наставак)");
                        Console.ReadKey();

                        break;
                    }
                case "5":
                    {
                        var users = await SqlRepository.GetAllUsers();

                        ConsoleTableBuilder
                            .From(users)
                            .WithTitle("КОРИСНИЦИ ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
                            .WithColumn("ИД", "Име", "Презиме", "Мејл", "Шифра")
                            .ExportAndWriteLine();

                        Console.WriteLine("Притисните било који тастер за наставак...");
                        Console.ReadKey();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Непозната опција. Покушајте поново..(притисните било који тастер за наставак)");
                        Console.ReadKey();
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Грешка: {ex.Message} {Environment.NewLine} (притисните било који тастер за наставак)");
            Console.ReadKey();
        }
    }
}

void ShowUserMenu()
{
    Console.Clear();
    Console.WriteLine("1. Додај");
    Console.WriteLine("2. Ажурирај");
    Console.WriteLine("3. Обриши");
    Console.WriteLine("4. Прикажи једног");
    Console.WriteLine("5. Прикажи све");
    Console.WriteLine("0. Назад");
    Console.Write("Одаберите опцију: ");
}