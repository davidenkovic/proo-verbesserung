# Verbesserung Booking | Ignjatovic David

## GetRoomsAsync()

_alter Code:_

```csharp
public async Task<List<RoomDTO>> GetRoomsAsync()
{
    return await _dbContext.Rooms.Select(r => new RoomDTO()
    {
        RoomNumber = r.RoomNumber,
        RoomType = r.RoomType,
        From = r.Bookings.OrderByDescending(b=> b.From.Year).Where(b => b.RoomId == r.Id && b.From.Year >= DateTime.Now.Year).Select(i => i.From).First(),
        To = r.Bookings.OrderByDescending(b => b.From.Year).Where(b => b.RoomId == r.Id && b.To > DateTime.Now).Select(i => i.To).First(),
        IsEmpty = r.Bookings.OrderByDescending(b => b.From.Year).Where(b => b.RoomId == r.Id).Select(i => i.From.Day > DateTime.Now.Day || i.From.Month > DateTime.Now.Month).Single(),
        CurrentBooking = r.Bookings.Single(b=> b.RoomId == r.Id)
    }).ToListAsync();
}
```

Man sieht, dass ich bei _GetRoomsAsync()_ es irgendwie geschaft habe eine Lösung zu bekommen welche auf keiner Art und weise schön ist.

_neuer Code:_

```csharp
public async Task<List<RoomDTO>> GetRoomsAsync()
{
    return await _dbContext.Rooms.Include(r => r.Bookings).ThenInclude(b => b.Customer).Select(r => new RoomDTO()
    {
        RoomNumber = r.RoomNumber,
        RoomType = r.RoomType,
        From = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First().From : null,
        To = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First().To : null,
        IsEmpty = r.Bookings != null ? CheckIsEmpty(r.Bookings.OrderByDescending(b => b.From).First()) : true,
        CurrentBooking = r.Bookings != null ? r.Bookings.OrderByDescending(b => b.From).First() : null
    })
        .ToListAsync();

}

public static bool CheckIsEmpty(Booking booking)
{
    if (booking.To == null || booking.To > DateTime.Now)
    {
        if (booking.From < DateTime.Now)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
    else
    {
        return true;
    }
}
```

Hier kann man sehen das ich eine neue Mehode geschrieben habe, welche mir das **richtige** überprüfen des Datums ermöglicht. Mit booking als Parameter kann ich leicht überprüfen ob der Raum am heutigen Tag zur verfügung steht. 

Es wurde berückichtigt das ein Customer in voraus Buchen kann. Somit kann eine beliebiger Customer davor noch einen Raum buchen.

Mit denn Elvis Operatoren habe ich leicht überprüfen können, ob die Buchung null ist oder nicht.

## Index.cshtml (Main page)

_alter Code:_
````html
<div class="row">
    <table class="table">
        <thead>
            <tr>
                <th>RoomNumber</th>
                <th>Roomtype</th>
                <th>IEmpty</th>
                <th>From</th>
                <th>To</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model.Rooms) {
                <tr>
                 <td>@item.RoomNumber</td>
                 <td>@item.RoomType</td>
                 <td>@item.IsEmpty</td>
                 <td>@item.From</td>
                 <td>@item.To</td>

                </tr>
            }
        </tbody>
    </table>
</div>

````

Die Klasse index.cshtml habe ich während der Prüfung nur schnell gemacht da ich keine Zeit hatte. Das wurde natürlich ersetzt um die Richtige Tabelle anzuzeigen.

_neuer Code:_

````html
<div class="row">
    <table class="table">
        <thead>
            <tr>
                <th>Nachname</th>
                <th>Vorname</th>
                <th>Anzahl Buchungen</th>
                <th>Bearbeiten</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var item in Model.Customers) {
                <tr>
                 <td>@item.LastName</td>
                 <td>@item.FirstName</td>
                 <td>@item.CountBookings</td>
                </tr>
            }
        </tbody>
    </table>
</div>
````

Diese Tabelle zeigt jetzt den Nachnamen, Vornamen und die Anzahl der Buchungen.

Wie man leicht bemerken kann hat sich meine Liste zum anzeigen geändert. Jetzt verwende ich eine neu erstelte DTO Klasse Namens _CustomerDTO_.

_neue Code:_
````csharp
public class CustomerDTO
{
    public string LastName { get; set; }    
    public string FirstName { get; set; }   
    public int CountBookings { get; set; }
    public int CustomerId { get; set; }
}
````

Diese wird dann in unserer Model Klasse verwendet und als Liste befüllt.

_neue Code:_
````csharp
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private IUnitOfWork _unitOfWork;

    [BindProperty]
    public List<CustomerDTO> Customers { get; set; } // neue Liste


    public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork; 
        Customers = new List<CustomerDTO>();
    }

    public async Task OnGet()
    {
        Customers = await _unitOfWork.Customers.GetCustomersForView(); // neue Methode in der Repository

    }
}
````

Das abfragen der Customer passiert mit einem einfachen Select welches dann das DTO befüllt.

_neue Code:_
````csharp
public async Task<List<CustomerDTO>> GetCustomersForView()
{
    return await _dbContext.Customers.Select(c => new CustomerDTO
    {
        CustomerId = c.Id,
        LastName = c.LastName,
        FirstName = c.FirstName,
        CountBookings = _dbContext.Bookings.Where(b => b.CustomerId == c.Id).Count()
    }).ToListAsync();
}
````

### Filter

Um die Filter Option zu erfüllen, fügen wir folgenden Code hinzu.

_neue Code:_
````csharp
<form method="post" class="form-inline">
    <div class="form-group">
        <label>Filter Name:</label>
        <input  class="form-control" type="text"/> @*asp-for="  "*@ <!--werte setzen in NameFiltered-->

        <label>Nur mit aktuellen Buchungen</label>
        <input class="form-control" type="checkbox"/> @*asp-for="actualBookingsChecked"*@ <!--werte setzen in ActualBookingsChecked-->

     </div>
    <div class="form-group">
        <input type="submit" value="Aktualisieren" class="btn btn-primary" />
    </div>
</form>

````

Die Form wird verwendet um die Tabelle zu filtern. Leider funktioniert sie noch nicht ganz.

Die Model Klasse wurde angepasst an der Form. 

_neue Code:_
````csharp
private readonly ILogger<IndexModel> _logger;
private IUnitOfWork _unitOfWork;

[BindProperty]
public List<CustomerDTO> Customers { get; set; }

[BindProperty]
public string NameFiltered { get; set; }


public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
{
    _logger = logger;
    _unitOfWork = unitOfWork; 
    Customers = new List<CustomerDTO>();
    NameFiltered = "alle";
}

public async Task OnGet()
{
    var list = await _unitOfWork.Customers.GetCustomersForView();
    foreach (var item in list)
    {
        if (NameFiltered == "alle")
        {
            Customers.Add(item);

        }
        if (item.FirstName.Contains(NameFiltered) ||item.LastName.Contains(NameFiltered))
        {
            Customers.Add(item);
        }
    }

}
````

Was hier noch nicht geht ist, dass nach dem button klick die Tablle leer ist. (leider keine kraft mehr um den Fehler zu suchen)

### Edit

Um ein Feld zu Editieren fügen wir eine weiter Spalte in die Tabelle ein.

````html
<td><a asp-page="/Customers/Edit" asp-route-id="@item.CustomerId">Bearbeiten</a></td>
````

Dieser link leitet uns auf unsere Edit seite und übergibt auch die CustomerId die benötoigt wird um einen Customer zu bearbeiten.

## Edit.cshtml

### Edit

In der Edit Model Klasse haben wir die Methode OnGetAsync() die mit dem Edit link aufgerufen wird.
Diese leifert die id mit.

Mit der Id können wir dann die Custommer bekommen.

_neue Code:_
````csharp
[BindProperty]
public Customer CurrentCustomer { get; set; }


public async Task<IActionResult> OnGetAsync(int? id)
{
    CurrentCustomer = await _uow.Customers.GetByIdAsync((int)id);
    
    return Page();
}
````

In Edit.cshtml können wir den Customer mithilfer der form anzeigen und auch bearbeiten. Das speichern passier mit einem Button.

_neue Code:_
````html
<div class="row">
<div class="col-md-4">
    <form method="post">
        <div class="form-group">
            <label>Vorname</label>
            <input asp-for="CurrentCustomer.FirstName" class="form-control" type="text"/>
            <span asp-validation-for="CurrentCustomer.FirstName" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label>Nachname4</label>
            <input asp-for="CurrentCustomer.LastName" class="form-control" type="text"/>
            <span asp-validation-for="CurrentCustomer.LastName" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label>Email-Adresse</label>
            <input asp-for="CurrentCustomer.EmailAddress" class="form-control" type="email"/>
            <span asp-validation-for="CurrentCustomer.EmailAddress" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label>Vorname</label>
            <input asp-for="CurrentCustomer.CreditCardNumber" class="form-control" type="text"/>
            <span asp-validation-for="CurrentCustomer.CreditCardNumber" class="text-danger"></span>
        </div>
        <div class="form-group">
            <input type="submit" value="Speichern" class="btn btn-primary" />
        </div>
    </form>
</div>
</div>
````

Nach dem der Button gedrückt wird, wird ein Post Request ausgeführt. Dieser "starten"dann die Methode _OnPostAsync()_.

_neue Code:_
````csharp
public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    try
    {
        await _uow.SaveChangesAsync();
        return Page();
    }
    catch (ValidationException e)
    {

        var validationResult = e.ValidationResult;
        foreach (var field in validationResult.MemberNames)
        {
            ModelState.AddModelError(field, validationResult.ToString());
        }

        ModelState.AddModelError("", e.Message);

        CurrentCustomer = await _uow.Customers.GetByIdAsync(CurrentCustomer.Id);

        return Page();
    }
}
````

### Bookings anzeigen

Um die Bookings eines Customers anzuzeigen, fügen wir zur unserer OnGetAsync() Methode noch etwas hinzu.

_neue Code:_
````csharp
public async Task<IActionResult> OnGetAsync(int? id)
{
    CurrentCustomer = await _uow.Customers.GetByIdAsync((int)id);
    CustomerBookings = await _uow.Bookings.GetBookingsForCustomer((int)id);
    
    return Page();
}
````

Die Methode GetBookingsForCustomer() hollt sich alle Bookings die zu dem jeweiligen Customer gehören.

_neue Code:_
````csharp

public async Task<List<BookingForCustomerDTO>> GetBookingsForCustomer(int customerId)
        {
            return await _dbContext.Bookings
                .Where(b => b.CustomerId == customerId)
                .Include(b => b.Customer)
                .Include(b => b.Room)
                .Select(b => new BookingForCustomerDTO
                {
                    From = b.From,
                    To = b.To,
                    RoomNumber = b.Room.RoomNumber,
                    RoomType = b.Room.RoomType.ToString(),
                    DaysSleeped = b.To != null ? (b.To - b.From).Value.TotalDays : -1
                }).ToListAsync();
        }
````

Problem hier ist das DaysSleep eine Comma Zahl ausgibt. Deshalb habe ich die Zahl gerundet.

````csharp
DaysSleeped = b.To != null ? Math.Round((b.To - b.From).Value.TotalDays,0) : -1
````

Die Buchungen werden in einer Tabelle angezeigt, nachdem sie in die Liste CustomerBookings gespeichert worden sind.

````html
<h4>Zimmerbuchungen</h4>
<table class="table">
 <thead>
        <tr>
            <th>
                Von
            </th>
            <th>
                Bis
            </th>
            <th>
                Zimmer
            </th>
            <th>
                Zimmertyp
            </th>
              <th>
                Anzahl Übernachtungen
            </th>
        </tr>
</thead>
<tbody>
    @foreach (var item in Model.CustomerBookings) {
            <tr>
                <td>@item.From</td>
                <td>@item.To</td>
                <td>@item.RoomNumber</td>
                <td>@item.RoomType</td>
                @if(@item.DaysSleeped == -1)
                {
                    <td></td>
                }
                @if(@item.DaysSleeped != -1)
                {
                    <td>@item.DaysSleeped</td>
                }
            </tr>
        }
</tbody>
</table>
````
In OnPostAsync() fügen wir noch die Zeile hinzu, um die Tabelle nach dem bearbeiten eines Customer zu Updaten.

````csharp
CustomerBookings = await _uow.Bookings.GetBookingsForCustomer(CurrentCustomer.Id);
````

## Edit

Das Edit passiert indem man auf den Speichern Button drück. Leider Funktionert er aber nicht.

Fügt man aber ```await _uow.Customers.AddAsync(CurrentCustomer);``` hinzu, müsste man die email ändern und somit wäre ein neuer Customer in unserer Tabelle.