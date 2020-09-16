import React, { Component } from "react";
import Association from "../../components/templates/Association";
import { UrlConstant } from "../../utils/Constants";

export default class extends Component {
  constructor(props) {
    super(props);
    let associations = [];
    for (let i = 0; i < 9; i++) {
      const associationItem = {
        id: i + 1,
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
        description:
          "Hội lập ra nhằm mục đích chia sẻ các kinh nghiệm trồng trái cây sạch cũng như quảng bá trái cây của nhóm, ngoài trái cây bạn còn có thể mua thêm tùm lum tà la ở đây",
        url: `${UrlConstant.Association.url}1`,
        followingNumber: "14",
        name: "Hội trái cây sạch An Thạnh",
        contentType: 4,
      };

      associations.push(associationItem);
    }

    const breadcrumbs = [
      {
        isActived: true,
        title: "Associations",
      },
    ];

    this.state = {
      associations,
      totalPage: 10,
      currentPage: 8,
      baseUrl: "/associations",
      breadcrumbs,
    };
  }

  render() {
    const {
      associations,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage,
    } = this.state;
    return (
      <Association
        associations={associations}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
