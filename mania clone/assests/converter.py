import cv2
import math
import time

unique_colors = 9

def downscale_and_extract_rgb(input_video_path, output_frame_size=(64, 48)):
    # Open the video file
    cap = cv2.VideoCapture(input_video_path)

    # Check if video opened successfully
    if not cap.isOpened():
        print("Error: Could not open video.")
        return

    frame_count = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    print(f"Total frames to process: {frame_count}")

    current_frame = 0
    outputSTR = ""
    start = time.perf_counter()
    while cap.isOpened():
        ret, frame = cap.read()  # Read a frame

        if not ret:
            print("Finished processing all frames.")
            break

        # Resize frame to the specified size
        resized_frame = cv2.resize(frame, output_frame_size, interpolation=cv2.INTER_AREA)

        # Loop through each pixel in the resized frame and extract RGB values
        
        height, width, _ = resized_frame.shape
        for y in range(height):
            for x in range(width):
                # Get the RGB value of the pixel
                b, g, r = resized_frame[y, x]  # OpenCV uses BGR by default
                #print(f"Frame {current_frame}, Pixel ({x}, {y}): R={r}, G={g}, B={b}")
                normalised = (r*unique_colors/255).__floor__()
                outputSTR += str(normalised)
            outputSTR += "#"
        outputSTR += "|"

        end = time.perf_counter()
        if (end-start) > 1:
            print(f"Processed frame {current_frame + 1}/{frame_count}")
            start = time.perf_counter()
        current_frame += 1
    with open("frameDataBig.txt","w") as f:
        f.write(outputSTR)
    # Release the video capture object
    cap.release()
    print("Video processing complete.")

# Usage example:
input_video = 'bad_apple.mp4'
res = 32
output_size = (8*res, 6*res)  # Example output size, you can change this
downscale_and_extract_rgb(input_video, output_size)