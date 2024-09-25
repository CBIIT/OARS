# Define the path to your project folder
$projectPath = "C:\ManishRathi\repos\TheradexGit\nci-web-reporting\Data\Models"  # Change this to your project's path

# Define the regex patterns for Column, Table, and Column with Key attributes
$columnPattern = '\[Column\("([a-zA-Z0-9_]+)"\)\]'
$tablePattern = '\[Table\("([a-zA-Z0-9_]+)",\s*Schema\s*=\s*"DMU"\s*\)\]'
$columnKeyPattern = '\[Column\("([a-zA-Z0-9_]+)"\),\s*Key\]'

# Get all .cs files in the project directory and its subdirectories
$csFiles = Get-ChildItem -Path $projectPath -Recurse -Include *.cs

foreach ($file in $csFiles) {
    # Read the content of the file
    $content = Get-Content $file.FullName -Raw
    
    # Check if the file contains Schema = "DMU"
    if ($content -match '\[Table\("([a-zA-Z0-9_]+)",\s*Schema\s*=\s*"DMU"\s*\)\]') {
        
        # Replace the column names with uppercase
        $content = [regex]::Replace($content, $columnPattern, {
            param($matches)
            return "[Column(`"" + $matches.Groups[1].Value.ToUpper() + "`")]"
        })

        # Replace the table names with uppercase
        $content = [regex]::Replace($content, $tablePattern, {
            param($matches)
            return "[Table(`"" + $matches.Groups[1].Value.ToUpper() + "`", Schema = `"DMU`")]"
        })

        # Replace the columns with [Column("column_name"), Key] pattern
        $content = [regex]::Replace($content, $columnKeyPattern, {
            param($matches)
            return "[Column(`"" + $matches.Groups[1].Value.ToUpper() + "`), Key]"
        })

        # Save the updated content back to the file
        Set-Content $file.FullName -Value $content
    }
}

Write-Output "Column, Table names, and [Column(...), Key] attributes have been updated to uppercase where Schema = 'DMU'."
