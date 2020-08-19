import React, { Component } from "react";
import Article from "../../components/templates/Article";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

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
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/fruit.jpg`,
        description:
          "Trao đổi với báo chí bên lề hội thảo khoa học “Sản xuất cây ăn quả bền vững” được tổ chức vào hôm qua, 28-3, tại tỉnh Tiền Giang, ông Lê Văn Thiệt, Phó cục trưởng Cục bảo vệ thực vật thuộc Bộ Nông nghiệp và Phát triển nông thôn cho biết, để gia tăng xuất khẩu trái cây, bắt buộc Việt Nam phải đàm phán mở cửa thị trường.",
        url: `${UrlConstant.News.url}1`,
        reactionNumber: "2.5k+",
        commentNumber: "14",
        name: "Hoa quả Việt muốn 'chinh phục' thế giới thì phải sạch",
        contentType: 1,
      };

      articles.push(articleItem);
    }

    const breadcrumbs = [
      {
        isActived: true,
        title: "News",
      },
    ];

    this.state = {
      articles,
      totalPage: 10,
      baseUrl: "/news",
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
