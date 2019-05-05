import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constant";
import { Pagination } from "../../components/molecules/Paging";
import ProductItem from "../../components/organisms/Product/ProductItem";

export default withRouter(
  class extends Component {
    constructor(props) {
      super(props);

      let products = [];
      for (let i = 0; i < 9; i++) {
        const productItem = {
          id: i + 1,
          creator: {
            photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
            profileUrl: "/trungle.it",
            name: "Ông 5 Đất"
          },
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
          farmUrl: `${UrlConstant.Farm.url}1`,
          farmName: "Trang trại ông năm đất",
          url: `${UrlConstant.Product.url}1`,
          commentNumber: "14",
          reactionNumber: "45+",
          name: "Chuối chính cây Đồng Nai",
          contentType: 2,
          price: 100000
        };

        products.push(productItem);
      }

      const { match } = this.props;
      const { params } = match;
      const { page } = params;

      this.state = {
        products,
        totalPage: 10,
        baseUrl: this.props.userUrl + "/products",
        currentPage: page ? page : 1
      };
    }

    render() {
      const { products, totalPage, baseUrl, currentPage } = this.state;
      return (
        <Fragment>
          <div className="row">
            {products
              ? products.map(item => (
                  <div
                    key={item.id}
                    className="col col-12 col-sm-6 col-md-6 col-lg-6 col-xl-4"
                  >
                    <ProductItem product={item} />
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
);
