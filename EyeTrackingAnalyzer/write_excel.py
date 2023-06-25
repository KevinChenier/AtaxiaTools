import pandas as pd
from openpyxl import Workbook
from openpyxl import load_workbook


def write_to_excel(excel_file, sheet_name, cell_reference, data):
    df = pd.DataFrame(data)
    with pd.ExcelWriter(excel_file, mode='a', engine='openpyxl') as writer:
        df.to_excel(writer, sheet_name=sheet_name, startrow=cell_reference[0]-1, startcol=cell_reference[1]-1, float_format="%.2f")

if __name__ == "__main__":
    write_to_excel("RV_Data.xlsx", "Metrics EyeTracking", [18,5], [12.0])