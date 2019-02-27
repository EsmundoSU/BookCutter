import cv2
import numpy as np
import math
from matplotlib import pyplot as plt

def GenerateBookMask( photoPath, cannyTresholdUp, cannyTresholdDown = 10, gaussianSize = 3 ):
    # Load image file from path
    img = cv2.imread(photoPath)
    # Grayscaled image
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
    # Add gaussian blur
    gauss = cv2.GaussianBlur(gray, (gaussianSize, gaussianSize), 0)
    # Canny - Edge Detection
    edges = cv2.Canny(gauss,cannyTresholdDown,cannyTresholdUp)
    #cv2.imshow("Photo - Closed Element", edges)
    #cv2.waitKey(0)

    # Generate closed area of book
    kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (7, 7))
    closed = cv2.morphologyEx(edges, cv2.MORPH_CLOSE, kernel)

    # Finding countours of book
    _, contours, hierarchy = cv2.findContours(closed, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)

    # Isolate largest contour
    contour_sizes = [(cv2.contourArea(contour), contour) for contour in contours]
    biggest_contour = max(contour_sizes, key=lambda x: x[0])[1]

    # Generate mask of book
    mask = np.zeros(closed.shape, np.uint8)
    cv2.drawContours(mask, [biggest_contour], -1, 255, -1)
    #cv2.imshow("Photo - Closed Element", mask)
    #cv2.waitKey(0)
    return mask