# Query Builder User Guide

## Overview

The Query Builder is a powerful tool that allows you to create custom queries across your business data without writing SQL code. You can filter, sort, group, and aggregate data from Loading, Unloading, Payment, and Party Billing registers.

---

## How to Use the Query Builder

### Step 1: Select a Module

At the top of the page, you'll see a **Module** dropdown. Select one of the following:

- **Loading Register** - All loading transactions
- **Unloading Register** - All unloading transactions
- **Payment Register** - All payment transactions
- **Party Bill Register** - All party billing transactions

### Step 2: Select Columns

Click on the **Column Selection** expander to expand it.

**Available Columns** (left side): Shows all fields available for the selected module.

**Selected Columns** (right side): Shows the columns that will appear in your results.

**Buttons:**
- **Add →** - Move selected column from Available to Selected
- **← Remove** - Remove selected column from Selected
- **▲ Up** - Move selected column up in the display order
- **▼ Down** - Move selected column down in the display order

**Tip:** You must select at least one column before executing a query.

### Step 3: Add Filters (Optional)

Click on the **Filters** expander to expand it.

Filters allow you to narrow down your results based on specific conditions.

**How to Add a Filter:**
1. Click **Add** button
2. A new row appears in the filter grid
3. Click on the **Field** cell to select which field to filter
4. Click on the **Operator** cell to select the comparison operator:
   - **Equals** - Exact match
   - **NotEquals** - Not equal to
   - **GreaterThan** - Greater than
   - **GreaterThanOrEqual** - Greater than or equal to
   - **LessThan** - Less than
   - **LessThanOrEqual** - Less than or equal to
   - **Contains** - Contains text (for text fields)
   - **StartsWith** - Starts with text (for text fields)
   - **EndsWith** - Ends with text (for text fields)
5. Enter the **Value** to compare against
6. For range operators (like GreaterThan), you may need a second value

**Example Filters:**
- Field: `BillAmount`, Operator: `GreaterThan`, Value: `10000` - Shows records with bill amount > 10,000
- Field: `CustomerName`, Operator: `Contains`, Value: `Transport` - Shows records where customer name contains "Transport"
- Field: `BillDate`, Operator: `GreaterThan`, Value: `2024-01-01` - Shows records after January 1, 2024

**Buttons:**
- **Add** - Add a new filter
- **Remove** - Remove selected filter
- **Clear** - Remove all filters

### Step 4: Add Sorting (Optional)

Click on the **Sorting** expander to expand it.

Sorting determines the order in which results appear.

**How to Add Sort:**
1. Click **Add** button
2. A new row appears in the sort grid
3. Click on the **Field** cell to select which field to sort by
4. Click on **Ascending** to toggle between ascending/descending order
5. The **Priority** column shows the sort order (0 = first, 1 = second, etc.)

**Example Sorting:**
- Field: `BillDate`, Ascending: `True`, Priority: `0` - Sort by date (oldest first)
- Field: `BillAmount`, Ascending: `False`, Priority: `1` - Then sort by amount (highest first)

**Buttons:**
- **Add** - Add a new sort
- **Remove** - Remove selected sort
- **Clear** - Remove all sorts

### Step 5: Add Grouping (Optional)

Click on the **Grouping** expander to expand it.

Grouping allows you to group results by a specific field and calculate aggregates.

**How to Add Grouping:**
1. Select a field from the dropdown
2. Results will be grouped by this field
3. You can add aggregates (see Step 6) to calculate totals per group

**Example Grouping:**
- Group by: `CustomerName` - Groups all records by customer
- Then add Sum aggregate on `BillAmount` - Shows total bill amount per customer

**Buttons:**
- **Clear** - Remove grouping

### Step 6: Add Aggregates (Optional)

Click on the **Aggregates** expander to expand it.

Aggregates allow you to calculate summary statistics.

**How to Add an Aggregate:**
1. Click **Add** button
2. A new row appears in the aggregate grid
3. Click on the **Field** cell to select which field to aggregate (must be a number field)
4. Click on the **Type** cell to select the aggregate type:
   - **Count** - Count of records
   - **Sum** - Sum of values
   - **Average** - Average of values
   - **Minimum** - Minimum value
   - **Maximum** - Maximum value
5. The **Display** column shows the display name for the aggregate

**Example Aggregates:**
- Field: `BillAmount`, Type: `Sum` - Total bill amount
- Field: `BillAmount`, Type: `Average` - Average bill amount
- Field: `BillDate`, Type: `Count` - Count of records

**Buttons:**
- **Add** - Add a new aggregate
- **Remove** - Remove selected aggregate
- **Clear** - Remove all aggregates

### Step 7: Search (Optional)

Use the **Search** text box to quickly search across all text fields.

**How Search Works:**
- Enter text to search for
- Search is applied across all text fields in the selected module
- Search uses "contains" logic (finds records where any text field contains your search term)
- Search is debounced (waits 300ms after you stop typing before executing)

**Example:**
- Search: `Transport` - Finds all records where any text field contains "Transport"

### Step 8: Execute Query

Click the **Execute Query** button to run your query.

**What Happens:**
- A "Executing query..." overlay appears
- The query runs on the database
- Results are displayed in the Results grid
- Record count and execution time are shown in the status bar

**Result Limit:**
- Maximum 10,000 records can be returned
- If your query returns more than 10,000 records, a warning banner appears
- Use filters to narrow your search if you hit the limit

### Step 9: View Results

Results are displayed in the Results grid at the bottom of the page.

**Grid Features:**
- Read-only (cannot edit results)
- Single selection mode
- Click column headers to sort
- Scroll horizontally if many columns

### Step 10: Export Results

Use the export buttons in the header to export your results:

- **Export PDF** - Export to PDF format
- **Export Excel** - Export to Excel (.xlsx) format
- **Export CSV** - Export to CSV format
- **Print** - Print the results

**Export Includes:**
- Company branding
- Module name
- Query metadata
- Timestamp
- Execution time
- Record count
- Formatted data

### Step 11: Reset

Click the **Reset** button to clear all settings and start fresh.

**Reset Clears:**
- Selected module
- Selected columns
- Filters
- Sorting
- Grouping
- Aggregates
- Search text
- Results

---

## Common Use Cases

### Use Case 1: View All Loading Transactions

1. Select Module: **Loading Register**
2. Select Columns: Select all columns you want to see
3. Click **Execute Query**

### Use Case 2: Find High-Value Bills

1. Select Module: **Party Bill Register**
2. Select Columns: BillNumber, CustomerName, BillAmount, BillDate
3. Add Filter: Field: `BillAmount`, Operator: `GreaterThan`, Value: `50000`
4. Add Sort: Field: `BillAmount`, Ascending: `False`
5. Click **Execute Query**

### Use Case 3: Group by Customer and Calculate Totals

1. Select Module: **Party Bill Register**
2. Select Columns: CustomerName, BillDate, BillAmount
3. Add Grouping: Select `CustomerName`
4. Add Aggregate: Field: `BillAmount`, Type: `Sum`
5. Add Aggregate: Field: `BillAmount`, Type: `Average`
6. Click **Execute Query**

### Use Case 4: Search for Specific Customer

1. Select Module: **Loading Register**
2. Select Columns: All columns you need
3. In Search box, type customer name (e.g., "Transport")
4. Click **Execute Query** (or wait for auto-search)

### Use Case 5: Date Range Filter

1. Select Module: **Payment Register**
2. Select Columns: All columns you need
3. Add Filter: Field: `PaymentDate`, Operator: `GreaterThanOrEqual`, Value: `2024-01-01`
4. Add Filter: Field: `PaymentDate`, Operator: `LessThanOrEqual`, Value: `2024-12-31`
5. Click **Execute Query**

---

## Tips and Best Practices

### Performance Tips
- **Use filters** to reduce the number of records returned
- **Select only necessary columns** - fewer columns = faster queries
- **Use sorting** instead of client-side sorting
- **Beware of the 10,000 record limit** - use filters if you hit it

### Navigation Tips
- **Scroll down** to see the Results grid (scrollbar appears on the right)
- **Expanders** (Column Selection, Filters, etc.) can be collapsed to save space
- **Header and Status Bar** stay fixed while you scroll the middle section

### Data Tips
- **Date format** - Use `yyyy-MM-dd` format for date values (e.g., `2024-01-15`)
- **Number format** - Use standard number format (e.g., `10000.50`)
- **Text search** - Search is case-insensitive
- **Null values** - Null values are handled automatically

### Export Tips
- **Excel** - Best for further analysis in Excel
- **PDF** - Best for printing and sharing
- **CSV** - Best for importing into other systems
- **Print** - Opens PDF in print dialog

---

## Troubleshooting

### Error: "Please select a module"
**Solution:** Select a module from the Module dropdown before executing the query.

### Error: "Please select at least one column"
**Solution:** Add at least one column to the Selected Columns list before executing the query.

### Error: "Results limited to 10,000 records"
**Solution:** Add more filters to narrow down your results to fewer than 10,000 records.

### Results grid is empty
**Possible causes:**
- No records match your filters
- Filters are too restrictive
- Wrong module selected
**Solution:** Try removing filters or selecting a different module.

### Columns don't appear in results
**Possible causes:**
- Columns not added to Selected Columns
- Column selection was cleared
**Solution:** Add the columns you want to see to the Selected Columns list.

### Query takes too long
**Possible causes:**
- Too many records being returned
- Complex filters
- No filters applied
**Solution:** Add filters to reduce the result set, select fewer columns.

### Threading Error (CollectionView)
**Solution:** This has been fixed in the latest version. All UI updates now happen on the UI thread.

---

## Field Reference

### Loading Register Fields
- BillNumber
- BillDate
- Consignor (Customer)
- Consignee (Customer)
- Source (SourceDestination)
- Destination (SourceDestination)
- Vehicle (Vehicle)
- Material (Material)
- Quantity
- Rate
- Amount
- DO Number
- DO Date
- PaymentLocation (PaymentLocation)
- UnionVendor (Vendor)
- Owner (VehicleOwner)
- ... and more

### Unloading Register Fields
- BillNumber
- BillDate
- Consignor (Customer)
- Consignee (Customer)
- Source (SourceDestination)
- Destination (SourceDestination)
- Vehicle (Vehicle)
- Material (Material)
- GrossWeightUL
- TareWeightUL
- UnloadingWeight
- ChallanMoney
- ... and more

### Payment Register Fields
- PaymentNumber
- PaymentDate
- Customer (Customer)
- Amount
- PaymentMode
- BankName
- ChequeNumber
- ... and more

### Party Bill Register Fields
- BillNumber
- BillDate
- Customer (Customer)
- Vehicle (Vehicle)
- LoadingCharge
- UnloadingCharge
- TransportCharge
- TotalAmount
- ... and more

---

## Support

If you encounter any issues or have questions about the Query Builder:

1. Check this user guide for common solutions
2. Review the error message in the ValidationMessage area
3. Try simplifying your query (remove filters, sorting, grouping)
4. Contact your system administrator

---

**Query Builder Version:** 1.0  
**Last Updated:** August 4, 2026
