export interface IProductAttributeValue {
  id?: number;
  name: string;
  description: string;
  statusId?: number;
  priceAdjustment?: number;
  pricePercentageAdjustment?: number;
  displayOrder?: number;
  [index: string]: any;
}

export interface IProductAttribute {
  attributeId?: number;
  id?: number;
  name: string;
  controlTypeId?: number;
  controlTypeName?: string;
  attributeRelationValues?: IProductAttributeValue[];
  [index: string]: any;
}
