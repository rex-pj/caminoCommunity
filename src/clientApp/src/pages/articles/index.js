import React, { Component } from "react";
import Article from "../../components/templates/Article";
import { UrlConstant } from "../../utils/Constants";

export default class extends Component {
  constructor() {
    super();

    let articles = [];
    for (let i = 0; i < 8; i++) {
      const articleItem = {
        id: i + 1,
        creator: {
          photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
          profileUrl: "/profile/4976920d11d17ddb37cd40c54330ba8e",
          name: "Anh Sáu",
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
        contentType: 1,
      };

      articles.push(articleItem);
    }

    const breadcrumbs = [
      {
        isActived: true,
        title: "Article",
      },
    ];

    this.state = {
      articles,
      totalPage: 10,
      baseUrl: "/articles",
      currentPage: 8,
      breadcrumbs,
    };
  }

  render() {
    const {
      articles,
      breadcrumbs,
      totalPage,
      baseUrl,
      currentPage,
    } = this.state;
    return (
      <Article
        articles={articles}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    );
  }
}
