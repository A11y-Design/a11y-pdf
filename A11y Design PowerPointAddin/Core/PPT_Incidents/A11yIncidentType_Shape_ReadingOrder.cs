using  Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A11y_Design_PowerPointAddin
{
    class A11yIncidentType_Shape_ReadingOrder : IA11yIncidentCreator<Slide>
    {

        private A11yIncidentTopic Topic { get; }
        IA11yIncidentTopic IA11yIncidentCreator<Slide>.Topic => Topic;

        private string Text;

        public A11yIncidentType_Shape_ReadingOrder(string text,A11yIncidentTopic topic)
        {
            Topic = topic;
            Text = text;
        }

        private List<Shape> readingOrderByCoord = new List<Shape>();

        public bool TryCreate(Slide slide, out IA11yIncidentItem errorItem)
        {
            readingOrderByCoord.Clear();
            errorItem = null;
            foreach (Shape _shape in slide.Shapes)
            {
                readingOrderByCoord.Add(_shape);
            }

            List<Shape> sortList = readingOrderByCoord
                .OrderBy(p => (int)p.Top)
                .ThenBy(p => (int)p.Left)
                .ToList<Shape>();

            foreach (var item in sortList.Select((value, i) => new { i, value }))
            {
                Shape _shape = (Shape)item.value;
                int position = item.i + 1;

                if (position != _shape.ZOrderPosition)
                {
                    errorItem = new A11yIncidentItem(
                        () => Helper.ShapeExtensions.SelectShape(_shape), 
                        Text, 
                        Topic);
                    return true;
                }
            }

            return false;

        }

        public void Reset()
        {
           readingOrderByCoord.Clear();
        }
    }
}