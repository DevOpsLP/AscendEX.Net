import os

def rename_files_and_folders_in_folder(folder_path):
    # Walk through the directory tree
    for dirpath, dirnames, filenames in os.walk(folder_path, topdown=False):
        # Rename files
        for filename in filenames:
            if "Coinbase" in filename:
                new_filename = filename.replace("Coinbase", "AscendEX")
                old_file_path = os.path.join(dirpath, filename)
                new_file_path = os.path.join(dirpath, new_filename)
                os.rename(old_file_path, new_file_path)
                print(f'Renamed file: {old_file_path} -> {new_file_path}')
        
        # Rename directories
        for dirname in dirnames:
            if "Coinbase" in dirname:
                new_dirname = dirname.replace("Coinbase", "AscendEX")
                old_dir_path = os.path.join(dirpath, dirname)
                new_dir_path = os.path.join(dirpath, new_dirname)
                os.rename(old_dir_path, new_dir_path)
                print(f'Renamed directory: {old_dir_path} -> {new_dir_path}')

# Replace 'your_folder_path' with the path to your folder
folder_path = '/Users/devluis/Documents/Projects/Coinbase.Net-1/'
rename_files_and_folders_in_folder(folder_path)
