namespace Omini.Opme.Be.Domain;

  internal class Item : Auditable
    {
        public Item(string description)
        {
            Description = description;
            _expenseReportItems = new List<ExpenseReportItem>();
        }

        public long Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Description { get; set; }
        public decimal Total { get; private set; }

        public IReadOnlyCollection<ExpenseReportItem> ExpenseReportItems => _expenseReportItems.ToArray();

        public void AddItem(ExpenseReportItem expenseReportItem)
        {
            Total += expenseReportItem.Amount;
            _expenseReportItems.Add(expenseReportItem);
        }

        public void RemoveItem(ExpenseReportItem expenseReportItem)
        {
            Total -= expenseReportItem.Amount;
            _expenseReportItems.Remove(expenseReportItem);
        }
    }