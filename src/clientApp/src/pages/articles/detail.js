import React, { Component, Fragment } from "react";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import Detail from "../../components/templates/Article/Detail";
import { UrlConstant } from "../../utils/Constants";

export default class extends Component {
  constructor() {
    super();

    const article = {
      title:
        "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
      createdDate: "25/03/2019 00:00",
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farmstay.jpg`,
      content:
        "Caroline Chaboo - nhà sinh vật học tại Đại học Kansas đánh giá rất cao tình mẫu tử của loài côn trùng này. Cô cho biết, những cá thể bọ rùa mẹ sẵn sàng bỏ ra 2 - 3 tuần để chiến đấu cùng con mình, hoặc thậm chí cho tới khi nào mối nguy hiểm thực sự qua đi. Những bà mẹ này không ngừng đề cao cảnh giác cho tới khi con mình đến tuổi trưởng thành. Bọ rùa mẹ luôn bảo vệ các con mình từ khi chúng mới nở ra từ trứng. Trong khoảng 2 tháng - thời gian trưởng thành của ấu trùng, bọ rùa mẹ luôn ở bên các con như hình với bóng để đảm bảo không một cá thể ấu trùng nào bị lạc ra khỏi đàn. Bên cạnh việc tự vũ trang cho mình bằng chất thải, trong thế giới loài bọ rùa còn có rất nhiều những thông tin thú vị.",
      commentNumber: "40+",
      reactionNumber: "2.5+",
      url: `${UrlConstant.Article.url}1`,
    };

    this.state = {
      article,
      breadcrumbs: [],
      relationArticles: [],
    };
  }

  componentDidMount() {
    const breadcrumbs = [
      {
        title: "Article",
        url: "/articles/",
      },
      {
        title: "Thiên địch",
        url: "/articles/thien-dich/",
      },
      {
        isActived: true,
        title:
          "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
      },
    ];

    this.setState({ breadcrumbs });
  }

  fetchRelationArticles = () => {
    let relationArticles = [];
    for (let i = 0; i < 6; i++) {
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
          "Bọ rùa (Coccinellidae), hay còn gọi là bọ hoàng hậu, bọ cánh cam là tên gọi chung cho các loài côn trùng nhỏ, mình tròn...",
        url: `${UrlConstant.Article.url}1`,
        reactionNumber: "2.5k+",
        commentNumber: "14",
        name:
          "Ban quản lý một siêu thị lớn tại Mỹ thả khoảng 72.000 con bọ rùa vào các gian hàng để chúng diệt rệp vừng.",
        contentType: 1,
      };

      relationArticles.push(articleItem);
    }

    this.setState({
      relationArticles,
    });
  };

  render() {
    const { breadcrumbs, article, relationArticles } = this.state;
    return (
      <Fragment>
        <Breadcrumb list={breadcrumbs} />
        <Detail
          article={article}
          fetchRelationArticles={this.fetchRelationArticles}
          relationArticles={relationArticles}
        />
      </Fragment>
    );
  }
}
