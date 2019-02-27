import tkinter as tk

class MainApplication:
    def __init__(self, master):
        self.master = master

        # ---------------------------------------------------------------
        # Top side of window
        self.topFrame = tk.Frame(self.master)
        self.topFrame.configure(bg="")

        # Radiobutton - Single Photo
        self.singlePhotoRadioButton = tk.Radiobutton(self.topFrame, value=1)
        self.singlePhotoRadioButton.select()

        # Radiobutton - Multiple Photos
        self.multiplePhotoRadioButton = tk.Radiobutton(self.topFrame, value=2)
        self.multiplePhotoRadioButton.deselect()

        # Label - "Single Photo"
        self.singlePhotoLabel = tk.Label(self.topFrame, text="Single Photo")

        # Label - "Multiple Photos"
        self.multiplePhotosLabel = tk.Label( self.topFrame, text="Multiple Photos")

        # Button - Open single photo
        self.openSinglePhotoButton = tk.Button( self.topFrame, text="Open Single...")

        # Button - Open multiple photos
        self.openMultiplePhotosButton = tk.Button( self.topFrame, text="Open Multiple...")

        # Button - Save single photo
        self.saveSinglePhotoButton = tk.Button( self.topFrame, text="Save Photo")
        
        # Button - Save multiple photos
        self.saveMultiplePhotosButton = tk.Button( self.topFrame, text="Save Photos")

        # Label - Programm information label
        self.infoLabel = tk.Label( self.topFrame, text="Info Label: ...TODO...")

        # Label - Empty Label for spacing
        self.spacingLabel = tk.Label( self.topFrame, bg="yellow", text="BLANK")

        # Label - Params Setup
        self.paramsSetupLabel = tk.Label( self.topFrame, text="Parameters Set-Up: ")

        # Button - Save Params
        self.paramsSaveButton = tk.Button( self.topFrame, text="Save params...")

        # Label - UpTreshold
        self.uptresholdLabel = tk.Label( self.topFrame, text = "UpperTreshold: ")

        # Gird Manegment
        self.singlePhotoRadioButton.grid( row = 0, column = 0 )
        self.multiplePhotoRadioButton.grid( row = 1, column = 0 )
        self.singlePhotoLabel.grid( row = 0, column = 1 )
        self.multiplePhotosLabel.grid( row = 1, column = 1 )
        self.openSinglePhotoButton.grid( row = 0, column = 2 )
        self.openMultiplePhotosButton.grid( row = 1, column = 2 )
        self.saveSinglePhotoButton.grid( row = 0, column = 3 )
        self.saveMultiplePhotosButton.grid( row = 1, column = 3 )
        self.infoLabel.grid( row = 2, column = 0, columnspan= 3 )
        self.spacingLabel.grid( row = 0, column = 4, rowspan = 3 )
        self.paramsSetupLabel.grid( row = 0, column = 5 )
        self.paramsSaveButton.grid( row = 0, column = 6, columnspan = 2)
        self.uptresholdLabel.grid( row = 1, column = 5 )

        self.topFrame.pack(side=tk.TOP, fill=tk.X, expand=1 )
        # ---------------------------------------------------------------

        # Bottom side of window
        self.bottomFrame = tk.Frame(self.master)
        self.bottomFrame.configure(bg="blue")


        self.bottomFrame.pack( side = tk.BOTTOM, fill=tk.BOTH, expand=1 )

def main(): 
    root = tk.Tk()
    root.geometry("1580x720")
    app = MainApplication(root)
    root.mainloop()

if __name__ == '__main__':
    main()