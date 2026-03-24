import os

directory = r"c:\Users\001\Downloads\CatchCells-devHibrido\CatchCells-devHibrido\Assets"
output_file = r"c:\Users\001\Downloads\CatchCells-devHibrido\CatchCells-devHibrido\all_code.txt"

with open(output_file, "w", encoding="utf-8") as out_f:
    for root, dirs, files in os.walk(directory):
        for file in files:
            if file.endswith(".cs"):
                path = os.path.join(root, file)
                out_f.write(f"\n\n--- FILE: {os.path.relpath(path, directory)} ---\n")
                try:
                    with open(path, "r", encoding="utf-8") as f:
                        out_f.write(f.read())
                except UnicodeDecodeError:
                    try:
                        with open(path, "r", encoding="windows-1252") as f:
                            out_f.write(f.read())
                    except Exception as e:
                        out_f.write(f"Error reading file: {e}\n")
                except Exception as e:
                    out_f.write(f"Error reading file: {e}\n")
print("Done writing to all_code.txt")
