import React, { Component } from "react";
import { UrlConstant } from "../../utils/Constants";
import Detail from "../../components/templates/Farm/Detail";

export default class extends Component {
  constructor(props) {
    super(props);

    const farm = {
      title: "Trang trại ông Năm Đất",
      createdDate: "25/03/2019 00:00",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm1.jpg`,
      content:
        "Trang trại nổi tiếng này từ lâu đã có mặt trong thành ngữ của người Long An, bất cứ cái gì to khủng khiếp, đều được ví với Waggoner, bởi vì nó rộng gần gấp đôi thành phố New York với 2000 cây số vuông nằm trải trên 9 hạt của bang Texas, điểm đặc biết nhất của Waggoner là nó thuộc sở hữu tư nhân kể từ ngày khai hoang năm 1849 đến nay. Trước đây, Waggoner chỉ về nhì trong cuộc đua diện tích trang trại, giữ ngôi quán quân là trang trại King, thuộc sở hữu của ông Richard King từ năm 1853, nhưng đến năm 1961, khối bất động sản rộng 3340 cây số vuông này trở thành tài sản lịch sử quốc gia Mỹ chứ không thuộc sở hữu tư nhân nữa. Với diện tích bằng 2/3 thành phố Hà Nội (3328 cây số vuông) trang trại Waggoner đang là bất động sản tư nhân lớn và đắt nhất thế giới với giá 725 triệu USD (15,7 nghìn tỷ đồng) , chủ nhân trang trại này hy vọng sẽ thu hút được các đại gia ở thung lũng Sillicon hoặc những ông trùm dầu mỏ Ả Rập. Ai mà không thích sở hữu mảnh đất to bằng cả một thành phố cơ chứ?",
      commentNumber: "40+",
      reactionNumber: "2.5+",
      url: `${UrlConstant.Farm.url}1`,
      address: "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An",
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

    this.state = { breadcrumbs: [], farm, farmProducts: [] };
  }

  fetchFarmProducts = () => {
    let farmProducts = [];
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

      farmProducts.push(productItem);
    }

    this.setState({
      farmProducts,
    });
  };

  componentDidMount() {
    const breadcrumbs = [
      {
        title: "Farms",
        url: "/farms/",
      },
      {
        isActived: true,
        title: "Trang trại ông Năm Đất",
      },
    ];

    this.setState({
      breadcrumbs,
    });
  }

  render() {
    const { breadcrumbs, farm, farmProducts } = this.state;
    return (
      <Detail
        farm={farm}
        breadcrumbs={breadcrumbs}
        fetchFarmProducts={this.fetchFarmProducts}
        farmProducts={farmProducts}
      />
    );
  }
}
