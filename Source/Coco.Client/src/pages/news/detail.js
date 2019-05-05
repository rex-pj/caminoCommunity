import React, { Component, Fragment } from "react";
import Breadcrumb from "../../components/molecules/Breadcrumb";
import Detail from "../../components/templates/Article/Detail";
import { UrlConstant } from "../../utils/Constant";

export default class extends Component {
  constructor(props) {
    super(props);

    const article = {
      title: "Hoa quả Việt muốn 'chinh phục' thế giới thì phải sạch",
      createdDate: "25/03/2019 00:00",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/fruit.jpg`,
      content: `Trao đổi với báo chí bên lề hội thảo khoa học “Sản xuất cây ăn quả bền vững” được tổ chức vào hôm qua, 28-3, tại tỉnh Tiền Giang, ông Lê Văn Thiệt, Phó cục trưởng Cục bảo vệ thực vật thuộc Bộ Nông nghiệp và Phát triển nông thôn cho biết, để gia tăng xuất khẩu trái cây, bắt buộc Việt Nam phải đàm phán mở cửa thị trường.
  
        Theo đó, ông Thiệt cho biết, đối với những thị trường khó tính như Úc, Mỹ, New Zealand, thậm chí ngay cả Trung Quốc, hiện muốn gia tăng xuất khẩu bắt buộc phải nộp hồ sơ mở cửa thị trường, trong đó, nội dung quan trọng cần phải làm là phân tích nguy cơ dịch hại. “Việc này đòi hỏi phải họp song phương rất nhiều lần và để một hồ sơ được thị trường chấp nhận mở cửa cho trái cây Việt Nam sang, thì mất thời gian rất lâu, từ 3-15 năm”, ông Thiệt cho biết.`,
      commentNumber: "40+",
      reactionNumber: "2.5+",
      url: `${UrlConstant.Article.url}1`
    };

    this.state = { breadcrumbs: [], article, relationNews: [] };
  }

  fetchRelationNews = () => {
    let relationNews = [];
    for (let i = 0; i < 6; i++) {
      const articleItem = {
        id: i + 1,
        creator: {
          photoUrl: `${process.env.PUBLIC_URL}/photos/farmer-avatar.jpg`,
          profileUrl: "/trungle.it",
          name: "Anh Sáu"
        },
        createdDate: "26/11/2018 9:28",
        updatedDate: "26/11/2018 9:28",
        thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farmstay.jpg`,
        description:
          "Bọ rùa (Coccinellidae), hay còn gọi là bọ hoàng hậu, bọ cánh cam là tên gọi chung cho các loài côn trùng nhỏ, mình tròn...",
        url: `${UrlConstant.Article.url}1`,
        reactionNumber: "2.5k+",
        commentNumber: "14",
        name:
          "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
        contentType: 1
      };

      relationNews.push(articleItem);
    }

    this.setState({
      relationNews
    });
  };

  componentDidMount() {
    const breadcrumbs = [
      {
        title: "Bài viết",
        url: "/news/"
      },
      {
        title: "Thiên địch",
        url: "/news/thien-dich/"
      },
      {
        isActived: true,
        title: "Hoa quả Việt muốn 'chinh phục' thế giới thì phải sạch"
      }
    ];

    this.setState({ breadcrumbs });
  }

  render() {
    const { breadcrumbs, article, relationNews } = this.state;
    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        <Detail
          article={article}
          fetchRelationArticles={this.fetchRelationNews}
          relationArticles={relationNews}
        />
      </Fragment>
    );
  }
}
