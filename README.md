 # a11y PDF - PowerPoint Addin for an accessible PDF-export

a11y pdf is an add-in for PowerPoint from a11y design GmbH that helps you analyse and improve the accessibility of PowerPoint presentations and allows you to export your presentation as accessible PDFs.

The analysis function of a11y pdf provides you with an automatically generated list of errors and notes, offering suggestions on how to optimise the accessibility of your PowerPoint presentation. With the help of these suggestions, you can modify your PowerPoint presentation so that it is accessible to most people with disabilities and also export it as a PDF. Even without any changes, the export function ensures a more accessible PDF than the native PowerPoint export.
When using a11y pdf, however, you should note that the a11y pdf add-in does not guarantee complete accessibility per se. The quality of the accessibility of your edited presentations ultimately depends on how you use the information and functions provided by a11y pdf to improve the accessibility of your presentations.

# Creating a PDF file that complies with PDF/UA 

## Requirements for accessible PowerPoint slides

In order to create an accessible PDF, it is necessary to ensure that various points are taken into account in PowerPoint, such as:
- correct reading order
- alternative text for images
- sufficient contrast between background colour and font colour
- and many more

The current version of PowerPoint already includes a very good accessibility check. You can change in a comfort way different things like alternative text reading order.
The user interface of a11y pdf is implemented with Windows Forms and is not as nicely designed as the integrated user interface of Office. Therefore, you are welcome to use the Office implementation.

## Requirements for PDF files that are PDF/UA compliant

The requirements for a PDF/UA-compliant file are very well documented online. Please refer to the relevant websites such as :
- PDF/UA-1 ([Matterhorn-Protocol](https://pdfa.org/resource/the-matterhorn-protocol/))


## How does the addin work?

There are three main functions:

1. Checking the PowerPoint slide for accessibility
2. Exporting the slides to a PDF file by using the default PDF/XPS export of PowerPoint
3. Editing the PDF file to ensure PDF/UA compliance by using the [itext](https://itextpdf.com/itext-suite-net-c)


# Installation of the add-in

There two possibilities for running the add-in:

- load the source and compile the add-in by using Visual Studio
- install the [published installer of the add-in](https://github.com/A11y-Design/a11y-pdf/releases)
