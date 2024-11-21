import pandas as pd
import os
import sys

def format_header(header):
    """
    Format header by removing spaces and underscores, and converting to Proper Case.
    Example: 'First Name' -> 'FirstName', 'first_name' -> 'FirstName'
    """
    # Replace underscores with spaces, split into words, and capitalize each word
    return ''.join(word.capitalize() for word in header.replace('_', ' ').split())

def excel_to_json(input_file, output_dir):
    # Check if the input file exists
    if not os.path.exists(input_file):
        print(f"Error: File '{input_file}' not found.")
        return

    # Create the output directory if it doesn't exist
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    # Read the Excel file
    try:
        excel_data = pd.ExcelFile(input_file)
    except Exception as e:
        print(f"Error reading Excel file: {e}")
        return

    # Iterate through each sheet
    for sheet_name in excel_data.sheet_names:
        try:
            # Load sheet into a DataFrame
            df = excel_data.parse(sheet_name)

            # Remove spaces, underscores, and make headers Proper Case
            df.columns = [format_header(col) for col in df.columns]

            # Convert the DataFrame to a JSON file
            json_file_path = os.path.join(output_dir, f"{sheet_name}.json")
            df.to_json(json_file_path, orient="records", indent=4)

            print(f"Converted '{sheet_name}' to '{json_file_path}'")
        except Exception as e:
            print(f"Error processing sheet '{sheet_name}': {e}")

if __name__ == "__main__":
    # Ensure the script is run with the correct arguments
    if len(sys.argv) != 3:
        print("Usage: python excel_to_json.py input_excel_file output_directory")
        sys.exit(1)

    input_excel_file = sys.argv[1]
    output_directory = sys.argv[2]

    excel_to_json(input_excel_file, output_directory)
