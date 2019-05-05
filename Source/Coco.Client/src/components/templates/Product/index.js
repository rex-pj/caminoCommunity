import React, { Component, Fragment } from "react";
import ProductItem from "../../organisms/Product/ProductItem";
import { Pagination } from "../../molecules/Paging";
import Breadcrumb from "../../molecules/Breadcrumb";

export default class extends Component {
  render() {
    const {
      products,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage
    } = this.props;

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
}
