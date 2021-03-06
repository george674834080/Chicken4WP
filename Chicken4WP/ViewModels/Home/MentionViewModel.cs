﻿using System.Collections.ObjectModel;
using Chicken4WP.Common;
using Chicken4WP.Models;
using Chicken4WP.Services.Interface;

namespace Chicken4WP.ViewModels.Home
{
    public class MentionViewModel : PivotItemViewModelBase
    {
        private ObservableCollection<Tweet> items;
        public ObservableCollection<Tweet> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        protected override void Initialize()
        {
            if (Items == null)
                Items = new ObservableCollection<Tweet>();
            RefreshData();
        }

        protected override void SetLanguage()
        {
            DisplayName = languageHelper.GetString("HomePage_Mention_Header");
        }

        protected override void RefreshData()
        {
            string sinceId = string.Empty;
            var option = new Option();
            if (Items.Count != 0)
            {
                sinceId = items[0].Id;
                option.Add(Const.SINCE_ID, sinceId);
            }
            tweetService.GetMentions(option,
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no new tweets yet
                        if (tweets.Count == 0)
                        {
                            toastMessageService.HandleMessage(languageHelper.GetString("Toast_Msg_NoNewMentions"));
                        }
                        #endregion
                        #region add
                        else
                        {
#if !LOCAL
                            if (sinceId == tweets[tweets.Count - 1].Id)
                                tweets.RemoveAt(tweets.Count - 1);
#endif
                            for (int i = tweets.Count - 1; i >= 0; i--)
                            {
                                Items.Insert(0, tweets[i]);
                                if (Items.Count >= Const.MAX_COUNT)
                                    Items.RemoveAt(Items.Count - 1);
                            }
                        }
                    }
                        #endregion
                    base.LoadDataCompleted();
                });
        }

        protected override void LoadData()
        {
            if (Items.Count == 0)
                return;
            string maxId = Items[Items.Count - 1].Id;
            var option = new Option();
            option.Add(Const.MAX_ID, maxId);
            tweetService.GetMentions(option,
                tweets =>
                {
                    if (!tweets.HasError)
                    {
                        #region no more tweets
                        if (tweets.Count == 0)
                        {
                            toastMessageService.HandleMessage(languageHelper.GetString("Toast_Msg_NoMoreMentions"));
                        }
                        #endregion
                        #region add
                        else
                        {
#if !LOCAL
                            if (maxId == tweets[0].Id)
                                tweets.RemoveAt(0);
#endif
                            foreach (var tweet in tweets)
                            {
                                Items.Add(tweet);
                            }
                        }
                        #endregion
                    }
                    base.LoadDataCompleted();
                });
        }
    }
}
