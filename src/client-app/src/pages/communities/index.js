import React, { useState } from "react";
import { DefaultLayout } from "../../components/templates/Layout";
import Community from "../../components/templates/Community";
import { UrlConstant } from "../../utils/Constants";

export default () => {
  let communities = [];
  for (let i = 0; i < 9; i++) {
    const communityItem = {
      id: i + 1,
      thumbnailUrl: `${process.env.PUBLIC_URL}/photos/farm-group-cover.jpg`,
      description:
        "Hội lập ra nhằm mục đích chia sẻ các kinh nghiệm trồng trái cây sạch cũng như quảng bá trái cây của nhóm, ngoài trái cây bạn còn có thể mua thêm tùm lum tà la ở đây",
      url: `${UrlConstant.Community.url}1`,
      followingNumber: "14",
      name: "Hội trái cây sạch An Thạnh",
      contentType: 4,
    };

    communities.push(communityItem);
  }

  const breadcrumbs = [
    {
      isActived: true,
      title: "Communities",
    },
  ];

  const [state] = useState({
    totalPage: 10,
    currentPage: 8,
    baseUrl: "/communities",
  });

  const { totalPage, baseUrl, currentPage } = state;
  return (
    <DefaultLayout>
      <Community
        communities={communities}
        breadcrumbs={breadcrumbs}
        totalPage={totalPage}
        baseUrl={baseUrl}
        currentPage={currentPage}
      />
    </DefaultLayout>
  );
};
