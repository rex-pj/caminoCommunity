import React, { Component } from "react";
import FarmGroup from "../../components/templates/FarmGroup";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

    let farmGroups = [];
    for (let i = 0; i < 9; i++) {
      const farmGroupItem = {
        id: i + 1,
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
        description:
          "Hội lập ra nhằm mục đích chia sẻ các kinh nghiệm trồng trái cây sạch cũng như quảng bá trái cây của nhóm, ngoài trái cây bạn còn có thể mua thêm tùm lum tà la ở đây",
        url: `${UrlConstant.FarmGroup.url}1`,
        followingNumber: "14",
        name: "Hội trái cây sạch An Thạnh",
        contentType: 4
      };

      farmGroups.push(farmGroupItem);
    }

    const breadcrumbs = [
      {
        isActived: true,
        title: "Nông hội"
      }
    ];

    this.state = {
      farmGroups,

      totalPage: 10,
      currentPage: 8,
      baseUrl: "/farm-groups",
      breadcrumbs
    };
  }

  render() {
    const {
      farmGroups,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage
    } = this.state;
    return (
      <FarmGroup
        farmGroups={farmGroups}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
