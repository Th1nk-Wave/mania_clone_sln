import cv2
import time

unique_colors = 255

def downscale_and_extract_rgb(input_video_path, output_frame_size=(64, 48),repeated_bytes_size=2):
    # Open the video file
    cap = cv2.VideoCapture(input_video_path)

    # Check if video opened successfully
    if not cap.isOpened():
        print("Error: Could not open video.")
        return

    frame_count = int(cap.get(cv2.CAP_PROP_FRAME_COUNT))
    print(f"Total frames to process: {frame_count}")

    current_frame = 0
    start = time.perf_counter()

    with open("frameDataCompressed.txt", "wb") as f:
        buffer = bytearray()
        while cap.isOpened():
            ret, frame = cap.read()  # Read a frame

            if not ret:
                print("Finished processing all frames.")
                break

            # Resize frame to the specified size
            resized_frame = cv2.resize(frame, output_frame_size, interpolation=cv2.INTER_AREA)

            # Loop through each pixel in the resized frame and extract RGB values
            height, width, _ = resized_frame.shape
            prevRGB = None
            repeated = 0

            for y in range(height):
                for x in range(width):
                    # Get the RGB value of the pixel
                    b, g, r = resized_frame[y, x]  # OpenCV uses BGR by default
                    currentRGB = (r, g, b)

                    # Normalize and handle repetitions
                    normalised = tuple((int(c) * unique_colors // 255) for c in currentRGB)

                    if currentRGB == prevRGB:
                        repeated += 1
                    else:
                        # Write the previous data if a new color is found
                        if prevRGB is not None:
                            buffer.extend(repeated.to_bytes(4, "little"))
                            buffer.extend(prevRGB[0].to_bytes(1, "little"))
                            buffer.extend(prevRGB[1].to_bytes(1, "little"))
                            buffer.extend(prevRGB[2].to_bytes(1, "little"))

                        # Reset repeated count and update previous RGB
                        repeated = 1
                        prevRGB = normalised

            # Write any remaining repeated data
            if prevRGB is not None:
                buffer.extend(repeated.to_bytes(4, "little"))
                buffer.extend(prevRGB[0].to_bytes(1, "little"))
                buffer.extend(prevRGB[1].to_bytes(1, "little"))
                buffer.extend(prevRGB[2].to_bytes(1, "little"))


            f.write(buffer)
            buffer.clear()

            end = time.perf_counter()
            if (end - start) > 1:
                print(f"Processed frame {current_frame + 1}/{frame_count}")
                start = time.perf_counter()

            current_frame += 1

    # Release the video capture object
    cap.release()
    print("Video processing complete.")

input_video = 'bad_apple.mp4'
res = 30
output_size = (8 * res, 6 * res)
downscale_and_extract_rgb(input_video, output_size)