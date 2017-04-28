# FXRatesConversionSystem
C# and WPF application to display RealTime FXRate Conversion

This is just a sample application which I developed for an interview quickly within hour so it is just for reference

Preliminary Requirements:

•	The below program developed in VS 2015 but you need at least VS.NET 2012 or above
•	Internet connection to access the online site (https://www.google.com/finance)

Idea:

•	Getting real-time currency rates from many currency pairs from https://www.google.com/finance asynchronously for set of many currency pairs with base currency USD.
•	I have dedicated services which will pull online rates after every 5 seconds. Using all kind of parallel commuting to fetch all updates in one go.
•	Once we have rates for all currency pairs (with base currency), applying calculation logic to get rates for all combinations of currency pairs.
•	An event-listener mechanism to listen updates and then display the rates frequently in speed to the WPF DataGrid.
•	Full use of MVVM design pattern, a different and dedicated view model for each cell in the grid to update each cell without any UI blocking

Challenges:

•	It is difficult to find any free service which offers real time updates within seconds. Initially I used many online sites to get real time data but they have protection for not to do screen scrapping and read data programmatically.
•	Then I used https://www.google.com/finance that gets rates with some delay (around 1 minute).

Assumptions:

•	Since rates from https://www.google.com/finance have some delay, in order to simulate quick updates on WPF GUI, I am updating rates manfully with some random minimum decimal value (0.01) after every 3 attempts (15 seconds) if rates are not changing within 15 seconds. This hard coding is not required but just to reflect real-time prices I am doing that. 
