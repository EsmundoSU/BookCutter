# ------------------------------------------------------------
# Load image
# ------------------------------------------------------------
img = cv2.imread('photos/book_2.jpg')
cv2.imshow("Photo - Default", img)
cv2.waitKey(0)
# ------------------------------------------------------------

# ------------------------------------------------------------
# Convert image
# ------------------------------------------------------------

# Grayscaled
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
cv2.imshow("Photo - Grayscaled", gray)
cv2.waitKey(0)

# Gaussian Blur
gauss = cv2.GaussianBlur(gray, (3, 3), 0)
cv2.imshow("Photo - Gaussian Blur", gauss)
cv2.waitKey(0)

# Canny - Edge Detection
edges = cv2.Canny(gauss,10,150)
cv2.imshow("Photo - Canny Edge Detection", edges)
cv2.waitKey(0)

# Closed element
kernel = cv2.getStructuringElement(cv2.MORPH_RECT, (7, 7))
closed = cv2.morphologyEx(edges, cv2.MORPH_CLOSE, kernel)
cv2.imshow("Photo - Closed Element", closed)
cv2.waitKey(0)

# Finding countours of book
_, contours, hierarchy = cv2.findContours(closed, cv2.RETR_LIST, cv2.CHAIN_APPROX_SIMPLE)

# Isolate largest contour
contour_sizes = [(cv2.contourArea(contour), contour) for contour in contours]
biggest_contour = max(contour_sizes, key=lambda x: x[0])[1]

# Generate mask of book
mask = np.zeros(closed.shape, np.uint8)
cv2.drawContours(mask, [biggest_contour], -1, 255, -1)
cv2.imshow("Photo - Mask", mask)
cv2.waitKey(0)

# ------------------------------------------------------------