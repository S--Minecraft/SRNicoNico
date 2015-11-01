﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using System.Windows.Controls;


using SRNicoNico.Models.NicoNicoWrapper;

namespace SRNicoNico.ViewModels {
    public class SearchViewModel : TabItemViewModel {

        //ソート方法
        private string[] sort_by = { "f:d", "f:a",
                                     "v:d", "v:a",
                                     "n:d", "n:a",
                                     "m:d", "m:a",
                                     "l:d", "l:a"
                                    };

        #region SearchText変更通知プロパティ
        private string _SearchText;

        public string SearchText {
            get { return _SearchText; }
            set {
                if (_SearchText == value)
                    return;
                _SearchText = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectedIndex変更通知プロパティ

        //バッキングストアはコンフィグ
        public int SelectedIndex {
            get { return Properties.Settings.Default.SearchIndex; }
            set {
                if (Properties.Settings.Default.SearchIndex == value)
                    return;
                Properties.Settings.Default.SearchIndex = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SearchType変更通知プロパティ
        private bool _SearchTypeText = true;
        private bool _SearchTypeTag;

        public string SearchType {
            get {
                if(_SearchTypeText) {
                    return "search";
                } else if (_SearchTypeTag) {
                    return "tag";
                } else {
                    return "search";
                }
            }
            set {
                if (value == "search" || value == "text") {
                    SearchTypeText = true;
                } else if(value == "tag") {
                    SearchTypeTag = true;
                } else {
                    SearchTypeText = true;
                }
            }
        }
        #region SearchType内部プロパティ
        //直接アクセスするのは非推奨
        public bool SearchTypeText
        {
            get { return _SearchTypeText; }
            set {
                if (_SearchTypeText == value) return;
                _SearchTypeText = value;
                _SearchTypeTag = !value;
                RaisePropertyChanged();
            }
        }
        //直接アクセスするのは非推奨
        public bool SearchTypeTag {
            get { return _SearchTypeTag; }
            set {
                if (_SearchTypeTag == value) return;
                _SearchTypeTag = value;
                _SearchTypeText = !value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #endregion

        private NicoNicoSearch currentSearch;


        #region SearchResult変更通知プロパティ
        private SearchResultViewModel _SearchResult;

        public SearchResultViewModel SearchResult {
            get { return _SearchResult; }
            set { 
                if(_SearchResult == value)
                    return;
                _SearchResult = value;
                RaisePropertyChanged();
            }
        }
        #endregion



        public SearchViewModel() : base("検索") { }

        //検索ボタン押下
        public void DoSearch() {


			if (SearchText == null || SearchText.Length == 0) {

				return;
			}

            SearchResult = new SearchResultViewModel();

            SearchResult.OwnerViewModel = this;

            SearchResult.IsActive = true;
            
            //検索
            currentSearch = new NicoNicoSearch(this, SearchText, SearchType, sort_by[SelectedIndex]);

			Task.Run(() => {

                NicoNicoSearchResult result = currentSearch.Search();
                //検索結果をUIに
                SearchResult.Total = string.Format("{0:#,0}", result.Total) + "件の動画";

                SearchResult.List.Clear();
                foreach(NicoNicoSearchResultEntry node in result.List) {

                    SearchResult.List.Add(new SearchResultEntryViewModel(node));
                }

                SearchResult.IsActive = false;
            });
		}

		public void SearchNext() {

            SearchResult.IsActive = true;
            Task.Run(() => {

                NicoNicoSearchResult result = currentSearch.Search();

                SearchResult.Total = string.Format("{0:#,0}", result.Total) + "件の動画";

                foreach(NicoNicoSearchResultEntry node in result.List) {

                    SearchResult.List.Add(new SearchResultEntryViewModel(node));
                }

                SearchResult.IsActive = false;
            });
        }
    }


}
