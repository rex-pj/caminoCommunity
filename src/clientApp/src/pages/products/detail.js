import React, { Component, Fragment } from "react";
import { UrlConstant } from "../../utils/Constant";
import Detail from "../../components/templates/Product/Detail";
import Breadcrumb from "../../components/molecules/Breadcrumb";

export default class extends Component {
  constructor(props) {
    super(props);

    const product = {
      title: "Chuối chính cây Đồng Nai",
      createdDate: "25/03/2019 00:00",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
      content:
        "Chúng ta đã nghe nói quá nhiều về những tác dụng tuyệt vời của chuối đối với sức khỏe, nhưng để duy trì việc ăn chuối thành thói quen tự nhiên hàng ngày thì vẫn rất ít người thực hiện được. Có thể bạn đã biết rõ tác dụng tốt của chuối, nhưng bạn vẫn chưa đủ động lực để ăn chuối hàng ngày, trong khi thói quen này lại có thể mang đến 5 thay đổi lớn đến các bộ phận cơ thể. Hãy tạo thói quen ăn chuối càng sớm càng tốt.",
      commentNumber: "40+",
      reactionNumber: "2.5+",
      url: `${UrlConstant.Farm.url}1`,
      address: "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An",
      price: 100000,
      origin: "Đồng Nai",
      farmUrl: `${UrlConstant.Farm.url}1`,
      farmName: "Trang trại ông năm đất",
      images: [
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana.jpg`,
        },
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana2.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana2.jpg`,
        },
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana3.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana3.jpg`,
        },
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana4.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana4.jpg`,
        },
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana5.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana5.jpg`,
        },
        {
          name: "Image 1",
          url: `${process.env.PUBLIC_URL}/photos/banana6.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/banana6.jpg`,
        },
        {
          name: "Peach",
          url: `${process.env.PUBLIC_URL}/photos/peach.png`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/peach.png`,
        },
        {
          name: "Fruit",
          url: `${process.env.PUBLIC_URL}/photos/fruit.jpg`,
          thumbnailUrl: `${process.env.PUBLIC_URL}/photos/fruit.jpg`,
        },
      ],
    };

    this.state = {
      product,
      breadcrumbs: [],
      relationProducts: [],
    };
  }

  fetchRelationProducts = () => {
    let relationProducts = [];
    for (let i = 0; i < 6; i++) {
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

      relationProducts.push(productItem);
    }

    this.setState({
      relationProducts,
    });
  };

  componentDidMount() {
    const breadcrumbs = [
      {
        title: "Products",
        url: "/products/",
      },
      {
        isActived: true,
        title: "Chuối chính cây Đồng Nai",
      },
    ];

    this.setState({
      breadcrumbs,
    });
  }

  render() {
    const { breadcrumbs, product, relationProducts } = this.state;
    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        <Detail
          product={product}
          breadcrumbs={breadcrumbs}
          fetchRelationProducts={this.fetchRelationProducts}
          relationProducts={relationProducts}
        />
      </Fragment>
    );
  }
}
