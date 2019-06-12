import React, { Component } from "react";
import Farm from "../../components/templates/Farm";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

    let farms = [];
    for (let i = 0; i < 9; i++) {
      const farmItem = {
        id: i + 1,
        creator: {
          photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
          profileUrl: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",
          name: "Ông 5 Đất"
        },
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm1.jpg`,
        description:
          "Trang trại nằm ở gần cầu Hàm Luông, có nuôi và trồng khá nhiều cây trồng vật nuôi, có cả homestay để nghĩ ngơi với những nhà sàn bên sông rất mát",
        url: `${UrlConstant.Farm.url}1`,
        commentNumber: "14",
        reactionNumber: "45+",
        name: "Trang trại ông Năm Đất",
        contentType: 3,
        address: "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An"
      };

      farms.push(farmItem);
    }

    const breadcrumbs = [
      {
        isActived: true,
        title: "Nông trại"
      }
    ];

    this.state = {
      farms,
      totalPage: 10,
      baseUrl: "/farms",
      currentPage: 8,
      breadcrumbs
    };
  }

  render() {
    const { farms, breadcrumbs, totalPage, baseUrl, currentPage } = this.state;
    return (
      <Farm
        farms={farms}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
