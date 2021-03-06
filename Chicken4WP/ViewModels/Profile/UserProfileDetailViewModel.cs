﻿using System.Collections.Generic;
using Chicken4WP.Common;
using Chicken4WP.Models;

namespace Chicken4WP.ViewModels.Profile
{
    public class UserProfileDetailViewModel : ProfileViewModelBase
    {
        private string profileImageBiggerUrl;
        public string ProfileImageBiggerUrl
        {
            get { return profileImageBiggerUrl; }
            set
            {
                profileImageBiggerUrl = value;
                NotifyOfPropertyChange(() => ProfileImageBiggerUrl);
            }
        }

        private string profileBannerUrl;
        public string ProfileBannerUrl
        {
            get { return profileBannerUrl; }
            set
            {
                profileBannerUrl = value;
                NotifyOfPropertyChange(() => ProfileBannerUrl);
            }
        }

        private string profileImageOriginalUrl;
        public string ProfileImageOriginalUrl
        {
            get { return profileImageOriginalUrl; }
            set
            {
                profileImageOriginalUrl = value;
                NotifyOfPropertyChange(() => ProfileImageOriginalUrl);
            }
        }

        private bool isFollowedBy;
        public bool IsFollowedBy
        {
            get
            {
                return isFollowedBy;
            }
            set
            {
                isFollowedBy = value;
                NotifyOfPropertyChange(() => IsFollowedBy);
            }
        }

        /// <summary>
        /// protected user's profile can be viewed,
        /// but tweets not.
        /// </summary>
        protected override void Initialize()
        {
            User = storageService.GetTempUser();
            RefreshData();
        }

        protected override void SetLanguage()
        {
            DisplayName = languageHelper.GetString("ProfilePage_ProfileDetail_Header");
        }

        protected override void ProfileRefreshData()
        {
            ProfileImageBiggerUrl = User.ProfileImageUrl.Replace("_normal", "_bigger");
            ProfileImageOriginalUrl = User.ProfileImageUrl.Replace("_normal", "");
            if (!string.IsNullOrEmpty(User.UserProfileBannerUrl))
                ProfileBannerUrl = User.UserProfileBannerUrl + "/web";
            GetFollowedByState();
        }

        protected override void LoadData()
        {

        }

        #region private
        private void GetFollowedByState()
        {
            tweetService.GetFriendshipConnections(User.Id,
                friendships =>
                {
                    if (!friendships.HasError && friendships.Count != 0)
                    {
                        List<string> connections = friendships[0].Connections;
                        if (connections.Contains(Const.FOLLOWED_BY))
                            IsFollowedBy = true;
                    }
                    base.LoadDataCompleted();
                });
        }
        #endregion
    }
}
