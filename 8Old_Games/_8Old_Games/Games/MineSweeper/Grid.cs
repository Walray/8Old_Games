using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.MineSweeper {
    class Grid
    {

        public bool isClicked = false; // 클릭 여부
        public bool isMine = false; // 폭탄 여부
        public bool isFlagged = false; // 깃발여부
        public int SurroundingMines = 0; // 근처에 폭탄 몇개인지! (즉, 주변에 폭탄 몇개 존재하는 지 카운트!
  


        public void AddMine()
        {
            isMine = true; // 이것은 폭탄
        }

        public void Open()
        {
            isClicked = true; // 클릭함

            if (isMine) // 클릭했을 때, 폭탄 이라면.
            { }

        }

        public void Flag()
        {
            if (!isClicked) // 클릭되지 않고(즉, 오른쪽클릭시)
            {
                isFlagged = !isFlagged; // 깃발이다(깃발 지정, 취소 해주어야하기 때문에 "!isFlagged" 값을 넣어주어야함)

            }
        }

     

    }
}
