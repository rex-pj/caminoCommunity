import React, { Component } from "react";
import Feeds from "../../components/templates/Feeds";
import { ContentType } from "../../utils/Enums";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

    const feeds = [];

    const articleItem = {
      id: "2",
      creator: {
        photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        profileUrl: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",
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
        "Bạn nghe nói đến nhiều tác dụng của chuối đối với sức khỏe, nhưng việc ăn chuối thường xuyên để có được 5 lợi ích đáng kinh ngạc này thì nhiều người chưa biết. Hãy sớm áp dụng.",
      name: "Đào ngâm thuốc sáu tháng không hư",
      id: "2353443435",
      createdDate: "4/12/2018",
      price: 100000,
      farmUrl: `${UrlConstant.Farm.url}1`,
      farmName: "Trang trại ông Chín Sớm",
      contentType: ContentType.Product,
      updatedDate: "4/12/2018",
      reactionNumber: "45+",
      commentNumber: "15+",
      url: `${UrlConstant.Product.url}1`,
      creator: {
        photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        profileUrl: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",
        name: "Bác Chín"
      }
    };

    const farmItem = {
      id: "3",
      creator: {
        photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
        profileUrl: "/profile?id=SXaSDRHRfds3zUDFQzC6jg==",
        name: "Ông 5 Đất",
        info: "Nông dân"
      },
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm1.jpg`,
      description:
        "Trang trại nằm ở gần cầu Hàm Luông, có nuôi và trồng khá nhiều cây trồng vật nuôi, có cả homestay để nghĩ ngơi với những nhà sàn bên sông rất mát",
      url: `${UrlConstant.Farm.url}1`,
      commentNumber: "14",
      reactionNumber: "45+",
      createdDate: "4/12/2018",
      name: "Trang trại ông Năm Đất",
      address: "123 Lò Sơn, ấp Gì Đó, xã Không Biết, huyện Cần Đước, Long An",
      contentType: 3
    };

    const farmGroupItem = {
      id: "4",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
      description:
        "Hội lập ra nhằm mục đích chia sẻ các kinh nghiệm trồng trái cây sạch cũng như quảng bá trái cây của nhóm",
      url: `${UrlConstant.FarmGroup.url}1`,
      followingNumber: "14",
      name: "Hội trái cây sạch An Thạnh",
      contentType: 4
    };

    feeds.push(articleItem);
    feeds.push(product);
    feeds.push(farmItem);
    feeds.push(farmGroupItem);

    feeds.push(articleItem);
    feeds.push(product);
    feeds.push(farmItem);
    feeds.push(farmGroupItem);

    this.state = {
      feeds,
      totalPage: 10,
      baseUrl: "/",
      currentPage: 1
    };
  }

  render() {
    const { feeds, totalPage, baseUrl, currentPage } = this.state;
    return (
      <Feeds
        feeds={feeds}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
