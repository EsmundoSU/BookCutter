import tkinter as tk
import bookFinder
import cv2
from PIL import ImageTk, Image

# Constants
basicImagePath = "photos/book_5.jpg"
maskImagePath = "printscreens/mask.png"

#create window & frames
class App:
    def __init__(self):
        self.root = tk.Tk()
        self._job = None
        
        self.slider = tk.Scale(self.root, from_=0, to=500, 
                               orient="horizontal", 
                               command=self.updateValue)
        self.slider.configure(width=50, length=600)
        self.slider.set(100)
        self.slider.pack()

        self.photoBasic = self.loadImage(basicImagePath)
        self.basicImageWidget = tk.Label(self.root, image = self.photoBasic )
        self.basicImageWidget.image = self.photoBasic
        self.basicImageWidget.pack(side="left")

        self.photoMask = self.loadMask(basicImagePath)
        self.maskImageWidget = tk.Label(self.root, image = self.photoMask )
        self.maskImageWidget.image = self.photoMask
        self.maskImageWidget.pack(side="right")

        self.root.mainloop()

    def updateValue(self, event):
        if self._job:
            self.root.after_cancel(self._job)
        self._job = self.root.after(500, self._do_something)

    def _do_something(self):
        self._job = None
        sliderValue = self.slider.get()
        mask = bookFinder.GenerateBookMask( basicImagePath, sliderValue )
        self.photoMask = ImageTk.PhotoImage( image= Image.fromarray(mask) )
        self.maskImageWidget.configure(image=self.photoMask)
        self.maskImageWidget.image = self.photoMask
        print("new value:", self.slider.get())

    def loadImage( self, imagePath ):
        image = Image.open(imagePath)
        photo = ImageTk.PhotoImage(image)
        return photo

    def loadMask( self, imagePath ):
        mask = bookFinder.GenerateBookMask( imagePath, self.slider.get() )
        photo = ImageTk.PhotoImage( image= Image.fromarray(mask) )
        return photo

app=App()