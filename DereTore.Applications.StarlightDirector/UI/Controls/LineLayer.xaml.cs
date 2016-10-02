﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DereTore.Applications.StarlightDirector.Components;

namespace DereTore.Applications.StarlightDirector.UI.Controls {
    public partial class LineLayer {

        public LineLayer() {
            InitializeComponent();
            NoteRelations = new NoteRelationCollection();
        }

        public NoteRelationCollection NoteRelations { get; }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
            DrawLines(drawingContext);
        }

        private void DrawLines(DrawingContext context) {
            var syncPen = new Pen(SyncRelationBrush, NoteLineThickness);
            var flickPen = new Pen(FlickRelationBrush, NoteLineThickness);
            var holdPen = new Pen(HoldRelationBrush, NoteLineThickness);
            foreach (var relation in NoteRelations) {
                var note1 = relation.ScoreNote1;
                var note2 = relation.ScoreNote2;
                Pen pen;
                switch (relation.Relation) {
                    case NoteRelation.None:
                        pen = null;
                        break;
                    case NoteRelation.Sync:
                        pen = syncPen;
                        break;
                    case NoteRelation.Flick:
                        pen = flickPen;
                        break;
                    case NoteRelation.Hold:
                        pen = holdPen;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(relation.Relation));
                }
                context.DrawLine(pen, new Point(note1.X, note1.Y), new Point(note2.X, note2.Y));
            }
        }

    }
}