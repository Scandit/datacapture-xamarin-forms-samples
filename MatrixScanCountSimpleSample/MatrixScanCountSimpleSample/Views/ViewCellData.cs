/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2023- Scandit AG. All rights reserved.
 */

using MatrixScanCountSimpleSample.Models;

namespace MatrixScanCountSimpleSample.Views
{
    public class ViewCellData
    {
        private int index;
        private ScanItem scanItem;

        public ViewCellData(ScanItem scanItem, int index)
        {
            this.scanItem = scanItem;
            this.index = index;
        }

        public string Detail => $"{this.scanItem.Symbology}: {this.scanItem.Barcode}";

        public bool ShouldShowQuantity => this.scanItem.Quantity > 1;

        public string Label
        {
            get
            {
                if (this.scanItem.Quantity > 1)
                {
                    return $"Non-unique item {this.index}";
                }

                return $"Item {this.index}";
            }
        }

        public string QuantityText => $"Qty: {this.scanItem.Quantity}";
    }
}
