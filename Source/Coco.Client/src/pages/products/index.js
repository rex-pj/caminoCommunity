import React, { Component } from "react";
import Product from "../../components/templates/Product";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

    let products = [];
    for (let i = 0; i < 9; i++) {
      const productItem = {
        id: i + 1,
        creator: {
          photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
          profileUrl: "/profile/4976920d11d17ddb37cd40c54330ba8e",
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

    const breadcrumbs = [
      {
        isActived: true,
        title: "Sản phẩm"
      }
    ];

    this.state = {
      products: products,
      totalPage: 10,
      baseUrl: "/products",
      currentPage: 8,
      breadcrumbs
    };
  }

  render() {
    const {
      products,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage
    } = this.state;
    return (
      <Product
        products={products}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
