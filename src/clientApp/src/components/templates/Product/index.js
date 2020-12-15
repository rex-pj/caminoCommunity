import React, { Fragment } from "react";
import ProductItem from "../../organisms/Product/ProductItem";
import { Pagination } from "../../organisms/Paging";
import Breadcrumb from "../../organisms/Navigation/Breadcrumb";

export default function (props) {
  const { products, breadcrumbs, totalPage, baseUrl, currentPage } = props;

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <div className="row">
        {products
          ? products.map((item, index) => (
              <div
                key={index}
                className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
              >
                <ProductItem key={item.id} product={item} />
              </div>
            ))
          : null}
      </div>
      <Pagination
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    </Fragment>
  );
}
