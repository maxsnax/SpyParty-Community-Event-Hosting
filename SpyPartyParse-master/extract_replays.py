# extract_replays.py

import sys
import os
import zipfile
import shutil
import uuid

def extract_flat_replays_by_leaf_dir(zip_path, output_dir):
    if not zipfile.is_zipfile(zip_path):
        print(f"ERROR: {zip_path} is not a valid zip file.")
        sys.exit(1)

    # Create output directory if it doesn't exist
    os.makedirs(output_dir, exist_ok=True)

    # Create a temp directory to extract all contents
    temp_extract_dir = os.path.join(output_dir, "temp_extract")
    os.makedirs(temp_extract_dir, exist_ok=True)

    # Extract all contents of the zip
    try:
        with zipfile.ZipFile(zip_path, 'r') as zip_ref:
            zip_ref.extractall(temp_extract_dir)
    except Exception as e:
        print(f"ERROR: Failed to extract zip file: {e}")
        sys.exit(1)

    replay_count = 0
    for root, _, files in os.walk(temp_extract_dir):
        replay_files = [f for f in files if f.lower().endswith('.replay')]
        if replay_files:
            # Current folder contains .replay files
            leaf_dir = os.path.basename(root)
            target_dir = os.path.join(output_dir, leaf_dir)
            os.makedirs(target_dir, exist_ok=True)

            for file in replay_files:
                source = os.path.join(root, file)
                destination = os.path.join(target_dir, file)

                # If a file with the same name exists, make it unique
                if os.path.exists(destination):
                    base, ext = os.path.splitext(file)
                    file = f"{base}_{uuid.uuid4().hex[:8]}{ext}"
                    destination = os.path.join(target_dir, file)

                try:
                    shutil.copy2(source, destination)
                    replay_count += 1
                except Exception as e:
                    print(f"WARNING: Failed to copy {file}: {e}")

    # Clean up temp extraction directory
    try:
        shutil.rmtree(temp_extract_dir)
    except Exception as cleanup_error:
        print(f"WARNING: Failed to clean temp folder: {cleanup_error}")

    if replay_count == 0:
        print("No .replay files found in the zip archive.")
        sys.exit(2)

    print(f"Extracted {replay_count} .replay files into their leaf directories at: {output_dir}")


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: extract_replays.py <zip_path> <output_dir>")
        sys.exit(1)

    zip_path = sys.argv[1]
    output_dir = sys.argv[2]

    extract_flat_replays_by_leaf_dir(zip_path, output_dir)
