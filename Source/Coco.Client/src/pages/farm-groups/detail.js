import React, { Component } from "react";
import FarmGroupLayout from "../../components/templates/Layout/FarmGroupLayout";
import Detail from "../../components/templates/FarmGroup/Detail";
import { UrlConstant } from "../../utils/Constant";
import Feeds from "../../components/templates/Feeds";
import { ContentType } from "../../utils/Enums";
import styled from "styled-components";

const Wrapper = styled.div`
  margin-top: ${p => p.theme.size.distance};
`;

export default class extends Component {
  constructor(props) {
    super(props);

    const farmGroup = {
      createdDate: "25/03/2019 00:00",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
      info: {
        url: `${UrlConstant.FarmGroup.url}1`,
        title: "Hội trái cây sạch An Thạnh",
        description:
          "Hội lập ra nhằm mục đích chia sẻ các kinh nghiệm trồng trái cây sạch cũng như quảng bá trái cây của nhóm, ngoài trái cây bạn còn có thể mua thêm tùm lum tà la ở đây",
        followingNumber: "14"
      }
    };

    this.state = {
      feeds: [],
      totalPage: 10,
      baseUrl: "/",
      currentPage: 1,
      breadcrumbs: [],
      farmGroup
    };
  }

  componentDidMount() {
    const breadcrumbs = [
      {
        title: "Nông hội",
        url: "/farm-groups/"
      },
      {
        isActived: true,
        title: "Hội trái cây sạch An Thạnh"
      }
    ];

    const feeds = [];

    const articleItem = {
      id: "2",
      creator: {
        photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        profileUrl: "/trungle.it",
        name: "Anh Sáu"
      },
      createdDate: "26/11/2018 9:28",
      updatedDate: "26/11/2018 9:28",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farmstay.jpg`,
      description:
        "Bọ rùa (Coccinellidae), hay còn gọi là bọ hoàng hậu, bọ cánh cam là tên gọi chung cho các loài côn trùng nhỏ, mình tròn hình cái trống, phủ giáp trụ, trên mặt cánh có những chấm đen (có loài không có). Người ta phân loại bọ rùa tùy theo số chấm và hình thái cơ thể Loài bọ rùa thường thấy nhất là bọ rùa 7 sao. Trên bộ cánh vỏ vàng cam có 7 nốt đen (mỗi cánh có ba nốt, còn một nốt ở chỗ giáp lại giữ hai cánh). Đây là loài bọ rùa to nhất và là một thợ săn đáng",
      url: `${UrlConstant.Article.url}1`,
      reactionNumber: "2.5k+",
      commentNumber: "14",
      name:
        "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
      contentType: ContentType.Article
    };

    const product = {
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/peach.png`,
      description:
        "Ambrosia có nghĩa là “thức ăn của các vị thần” trong thần thoại Hy Lạp cổ đại và được lựa chọn bởi Wilfrid Mennell và vợ, họ phát hiện ra cây táo Ambrosia gốc trong vườn tại",
      name: "Đào ngâm thuốc sáu tháng không hư",
      id: "2353443435",
      createdDate: "4/12/2018",
      price: 100000,
      farmUrl: `${UrlConstant.Farm.url}1`,
      farmName: "Trang trại ông Chín Sớm",
      contentType: ContentType.Product,
      updatedDate: "4/12/2018",
      reactionNumber: "45+",
      url: `${UrlConstant.Product.url}1`,
      creator: {
        photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        profileUrl: "/trungle.it",
        name: "Bác Chín"
      }
    };

    feeds.push(articleItem);
    feeds.push(product);

    feeds.push(articleItem);
    feeds.push(product);

    feeds.push(articleItem);
    feeds.push(product);

    feeds.push(articleItem);
    feeds.push(product);

    this.setState({
      breadcrumbs,
      feeds
    });
  }

  render() {
    const {
      breadcrumbs,
      farmGroup,
      feeds,
      totalPage,
      baseUrl,
      currentPage
    } = this.state;

    return (
      <FarmGroupLayout info={farmGroup.info}>
        <Detail farmGroup={farmGroup} breadcrumbs={breadcrumbs} />
        <Wrapper>
          <Feeds
            feeds={feeds}
            totalPage={totalPage}
            baseUrl={baseUrl}
            currentPage={currentPage}
          />
        </Wrapper>
      </FarmGroupLayout>
    );
  }
}
