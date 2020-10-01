import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";
import { UrlConstant } from "../../utils/Constants";
import { Pagination } from "../../components/organisms/Paging";
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
            profileUrl: "/profile/4976920d11d17ddb37cd40c54330ba8e",
            name: "Ông 5 Đất",
          },
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
          farmUrl: `${UrlConstant.Farm.url}1`,
          farmName: "Trang trại ông năm đất",
          url: `${UrlConstant.Product.url}1`,
          commentNumber: "14",
          reactionNumber: "45+",
          name: "Chuối chính cây Đồng Nai",
          contentType: 2,
          price: 100000,
        };

        products.push(productItem);
      }

      const { location, pageNumber } = this.props;

      this.state = {
        products,
        totalPage: 10,
        baseUrl: this.props.userUrl + "/products",
        pageQuery: location.search,
        currentPage: pageNumber ? pageNumber : 1,
      };
    }

    render() {
      const {
        products,
        totalPage,
        baseUrl,
        currentPage,
        pageQuery,
      } = this.state;

      return (
        <Fragment>
          <div className="row">
            {products
              ? products.map((item) => (
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
            pageQuery={pageQuery}
            currentPage={currentPage}
          />
        </Fragment>
      );
    }
  }
);
