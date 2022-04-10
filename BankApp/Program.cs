/**
 * Operacje:
 * Administrator
 * Dodawanie / usuwanie klientów do systemu <- Admin dostanie uchwyt na listę userów z rolami i będzie miał metodę dodawania userów
 * Modyfikowanie ustawień klientów <- metoda admina
 * Raport operacji wybranego klienta <- User będzie miał listę
 *
 * 
 * Klient: zasilenie konta w bankomacie <- metoda zwyklego usera
 * Klient: przelewy wychodzące / przychodzące (walidacja numeru na który wykonywany jest przelew (należy wymyślić własny format z "bitem parzystości") <- wykonaj przelew metoda zwyklego usera
 * Klient: kredyty (zasila konto o wybraną kwotę, konieczność spłaty wnioskowanej kwoty + %) <-  metoda zwyklego usera
 * Klient: przeglądanie historia operacji <- przejrzenie historii operacji
 *
 *
 */

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User user = new Client("new", "client", new UserSettings("PLN", "Polish"));
        }
    }
}
