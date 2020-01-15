import React, { useState, useEffect, useContext } from "react";
import { withRouter } from "react-router-dom";
import Profile from "../../components/organisms/User/Profile";
import { SessionContext } from "../../store/context/SessionContext";
import { publicClient } from "../../utils/GraphQLClient";
import { GET_USER_INFO } from "../../utils/GraphQLQueries";
import { useQuery } from "@apollo/react-hooks";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import Loading from "../../components/atoms/Loading";
import { useStore } from "../../store/hook-store";

export default withRouter(props => {
  const [isEditCoverMode, setEditCoverMode] = useState(false);
  const _baseUrl = "/profile";
  const sessionContext = useContext(SessionContext);
  const { match } = props;
  const { params } = match;
  const { userId, pageNumber } = params;
  const { loading, error, data, refetch } = useQuery(GET_USER_INFO, {
    client: publicClient,
    variables: {
      criterias: {
        userId
      }
    }
  });

  const pages = [
    {
      path: [`${_baseUrl}/:userId/about`],
      dir: "user-about"
    },
    {
      path: [`${_baseUrl}/:userId/update`],
      dir: "user-update"
    },
    {
      path: [`${_baseUrl}/:userId/security`],
      dir: "user-security"
    },
    {
      path: [
        `${_baseUrl}/:userId/posts`,
        `${_baseUrl}/:userId/posts/page/:pageNumber`
      ],
      dir: "user-posts"
    },
    {
      path: [
        `${_baseUrl}/:userId/products`,
        `${_baseUrl}/:userId/products/page/:pageNumber`
      ],
      dir: "user-products"
    },
    {
      path: [
        `${_baseUrl}/:userId/farms`,
        `${_baseUrl}/:userId/farms/page/:pageNumber`
      ],
      dir: "user-farms"
    },
    {
      path: [
        `${_baseUrl}/:userId/followings`,
        `${_baseUrl}/:userId/followings/page/:pageNumber`
      ],
      dir: "user-followings"
    },
    {
      path: [
        `${_baseUrl}/:userId`,
        `${_baseUrl}/:userId/feeds`,
        `${_baseUrl}/:userId/feeds/page/:pageNumber`
      ],
      dir: "user-feeds"
    }
  ];

  const onToggleEditCoverMode = e => {
    setEditCoverMode(e);
  };

  const [state, dispatch] = useStore(false);
  if (state.type === "AVATAR_UPDATED") {
    refetch();
  }

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error"
    });
  };

  const userCoverUpdated = async (action, data) => {
    console.log(action);
    console.log(data);
    if (data && data.canEdit) {
      return await action({ variables: { criterias: data } })
        .then(async () => {
          await refetch().then(() => {
            if (sessionContext.relogin) {
              sessionContext.relogin();
            }
          });
        })
        .catch(() => {
          showValidationError(
            "Có lỗi xảy ra",
            "Có lỗi xảy ra khi cập nhật dữ liệu, bạn vui lòng thử lại"
          );
        });
    }
  };

  useEffect(() => {}, [refetch, sessionContext.relogin]);

  if (loading) {
    return <Loading>Loading</Loading>;
  }
  if (error) {
    return <ErrorBlock>Error</ErrorBlock>;
  }

  const parseUserInfo = response => {
    const { fullUserInfo } = response;
    const { result, accessMode } = fullUserInfo;
    const canEdit = accessMode === "CAN_EDIT";

    return {
      ...result,
      canEdit: canEdit,
      url: result.userIdentityId ? `/profile/${result.userIdentityId}` : ""
    };
  };

  const userInfo = parseUserInfo(data);

  return (
    <Profile
      isEditCoverMode={isEditCoverMode}
      userId={userId}
      pageNumber={pageNumber}
      baseUrl={_baseUrl}
      onToggleEditCoverMode={onToggleEditCoverMode}
      userCoverUpdated={userCoverUpdated}
      showValidationError={showValidationError}
      pages={pages}
      userInfo={userInfo}
    />
  );
});
