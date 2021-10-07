import React, { Fragment } from "react";
import ProductItem from "../../organisms/Product/ProductItem";

export default function (props) {
  const { products, onOpenDeleteConfirmation } = props;

  return (
    <Fragment>
      <div className="row gx-1">
        {products
          ? products.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4 px-2"
              >
                <ProductItem
                  key={item.id}
                  product={item}
                  onOpenDeleteConfirmationModal={onOpenDeleteConfirmation}
                />
              </div>
            ))
          : null}
      </div>
    </Fragment>
  );
}
