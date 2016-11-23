using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8Old_Games.Games.Sudoku {
    public class MapData {
        public MapData() {; }
        public int[, ] getMap(int stage, int num) {
            switch (stage) {
                case 0:
                    if (num == 0) return e1_answer;
                    if (num == 1) return e2_answer;
                    if (num == 2) return e3_answer;
                    if (num == 3) return e4_answer;
                    if (num == 4) return e5_answer;
                    break;
                case 1:
                    if (num == 0) return n1_answer;
                    if (num == 1) return n2_answer;
                    if (num == 2) return n3_answer;
                    if (num == 3) return n4_answer;
                    if (num == 4) return n5_answer;
                    break;
                case 2:
                    if (num == 0) return h1_answer;
                    if (num == 1) return h2_answer;
                    if (num == 2) return h3_answer;
                    if (num == 3) return h4_answer;
                    if (num == 4) return h5_answer;
                    break;
                case 3:
                    if (num == 0) return x1_answer;
                    if (num == 1) return x2_answer;
                    if (num == 2) return x3_answer;
                    if (num == 3) return x4_answer;
                    if (num == 4) return x5_answer;
                    break;
            }
            return e1_answer;
        }

        public bool[,] getVisible(int stage, int num) {
            switch (stage) {
                case 0:
                    if (num == 0) return e1_visible;
                    if (num == 1) return e2_visible;
                    if (num == 2) return e3_visible;
                    if (num == 3) return e4_visible;
                    if (num == 4) return e5_visible;
                    break;
                case 1:
                    if (num == 0) return n1_visible;
                    if (num == 1) return n2_visible;
                    if (num == 2) return n3_visible;
                    if (num == 3) return n4_visible;
                    if (num == 4) return n5_visible;
                    break;
                case 2:
                    if (num == 0) return h1_visible;
                    if (num == 1) return h2_visible;
                    if (num == 2) return h3_visible;
                    if (num == 3) return h4_visible;
                    if (num == 4) return h5_visible;
                    break;
                case 3:
                    if (num == 0) return x1_visible;
                    if (num == 1) return x2_visible;
                    if (num == 2) return x3_visible;
                    if (num == 3) return x4_visible;
                    if (num == 4) return x5_visible;
                    break;
            }
            return e1_visible;
        }

        int[,] e1_answer = { { 4, 5, 7, 3, 8, 2, 9, 1, 6 },
                          { 8, 9 ,1, 4, 6, 7, 2 ,5 ,3 },
                          { 2, 6 ,3, 1, 9, 5 ,8 ,7 ,4 },
                          { 7, 1, 2, 8, 4 ,3, 6, 9, 5 },
                          { 6, 8, 4, 5, 1, 9, 3, 2, 7 },
                          { 9, 3, 5, 7, 2, 6, 4, 8, 1 },
                          { 5, 7, 9, 2 ,3 ,4, 1, 6 ,8 },
                          { 3, 2, 8, 6, 7, 1, 5, 4, 9 },
                          { 1, 4, 6, 9, 5 ,8, 7, 3, 2 }
        };

        bool[,] e1_visible = { { false, true, true, true, true, false, false, false, false },
                          { false, false, true, false, false, true, false, true, false},
                          { false, false, false, false, true, false, true, false, true},
                          { false, true, true, true, false, false, false, true, false },
                          { false, false, false, false, true, false, false, false, false },
                          { false, true, false, false, false, true, true, true, false },
                          { true, false, true, false, true, false, false, false, false },
                          { false, true, false, true, false, false, true, false, false },
                          { false, false, false, false, true, true, true, true, false }

        };


        int[,] e2_answer = { { 3, 4, 7, 1, 9, 2, 5, 8, 6 },
                          { 9, 1, 6, 8, 5, 4, 7, 3, 2 },
                          { 8, 2, 5, 3, 7, 6 ,1, 9, 4 },
                          { 7, 5, 4, 2, 6 ,8, 9, 1, 3 },
                          { 6, 8, 9, 5, 3, 1, 2, 4, 7 },
                          { 1, 3, 2, 9, 4, 7, 8, 6, 5 },
                          { 4, 9, 1, 6, 2, 5, 3, 7, 8 },
                          { 2, 7, 3, 4, 8, 9, 6, 5, 1 },
                          { 5, 6, 8, 7, 1 ,3, 4, 2, 9 }
        };

        bool[,] e2_visible = { { true, true, false, false, false, false, true, false, false },
                          { true, true, false, false, true, true, true, true, false},
                          { true, false, false, false, false, false, false, false, true},
                          { true, false, false, true, false, false, false, true, false },
                          { false, false, false, false, true, false, false, false, false },
                          { false, true, false, false, false, true, false, false, true },
                          { true, false, false, false, false, false, false, false, true },
                          { false, true, true, true, true, false, false, true, true },
                          { false, false, true, false, false, false, false, true, true }

        };


        int[,] e3_answer = { { 4, 9, 8, 6, 2, 5, 7, 3, 1 },
                          { 2, 6, 5, 7, 1, 3, 9, 8, 4 },
                          { 7, 1, 3, 8, 9, 4 ,2, 5, 6 },
                          { 8, 3, 7, 2, 6 ,1, 4, 9, 5 },
                          { 6, 4, 9, 5, 3, 7, 1, 2, 8 },
                          { 5, 2, 1, 9, 4, 8, 6, 7, 3 },
                          { 3, 7, 4, 1, 8, 9, 5, 6, 2 },
                          { 1, 5, 2, 3, 7, 6, 8, 4, 9 },
                          { 9, 8, 6, 4, 5, 2, 3, 1, 7 }
        };

        bool[,] e3_visible = { { false, true, false, true, false, false, true, false, true },
                          { true, false, false, false, false, true, false, true, true },
                          { true, false, true, false, false, false, false, false, false},
                          { false, true, false, false, true, true, false, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, false, true, true, false, false, true, false },
                          { false, false, false, false, false, false, true, false, true },
                          { true, true, false, true, false, false, false, false, true },
                          { true, false, true, false, false, true, false, true, false }

        };


        int[,] e4_answer = { { 8, 9, 2, 3, 6, 1, 7, 5, 4 },
                          { 4, 7, 6, 9, 5, 2, 1, 8, 3 },
                          { 5, 1, 3, 7, 4, 8, 2, 9, 6 },
                          { 6, 5, 7, 4, 1, 3, 9, 2, 8 },
                          { 2, 4, 9, 6, 8, 5, 3, 7, 1 },
                          { 1, 3, 8, 2, 7, 9, 4, 6, 5 },
                          { 7, 8, 4, 1, 9, 6, 5, 3, 2 },
                          { 9, 2, 5, 8, 3, 4, 6, 1, 7 },
                          { 3, 6, 1, 5, 2, 7, 8, 4, 9 }
        };

        bool[,] e4_visible = { { false, true, false, true, false, true, false, true, false },
                          { false, false, false, true, false, true, false, false, false },
                          { true, true, false, false, false, false, false, true, true },
                          { false, false, true, true, false, true, true, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, true, true, false, true, true, false, false },
                          { true, true, false, false, false, false, false, true, true },
                          { false, false, false, true, false, true, false, false, false },
                          { false, true, false, true, false, true, false, true, false }

        };


        int[,] e5_answer = { { 7, 6, 4, 1, 5, 9, 3, 8, 2 },
                          { 5, 1, 9, 2, 3, 8, 7, 4, 6 },
                          { 8, 2, 3, 6, 7, 4, 9, 5, 1 },
                          { 4, 5, 7, 9, 8, 2, 6, 1, 3 },
                          { 9, 3, 6, 7, 1, 5, 4, 2, 8 },
                          { 2, 8, 1, 4, 6, 3, 5, 9, 7 },
                          { 1, 4, 2, 3, 9, 7, 8, 6, 5 },
                          { 6, 7, 5, 8, 4, 1, 2, 3, 9 },
                          { 3, 9, 8, 5, 2, 6, 1, 7, 4 }
        };

        bool[,] e5_visible = { { false, true, false, true, false, true, false, true, false },
                          { false, true, true, false, false, false, true, true, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, true, true, false, true, true, false, false },
                          { false, true, false, false, false, false, false, true, false },
                          { false, false, true, true, false, true, true, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, true, true, false, false, false, true, true, false },
                          { false, true, false, true, false, true, false, true, false }

        };










        int[,] n1_answer = { { 5, 9, 2, 4, 3, 6, 1, 7, 8 },
                          { 7, 8, 4, 2, 1, 5, 9, 3, 6 },
                          { 1, 3, 6, 9, 8, 7, 5, 4, 2 },
                          { 3, 2, 7, 5, 6, 8, 4, 1, 9 },
                          { 4, 5, 1, 3, 2, 9, 6, 8, 7 },
                          { 8, 6, 9, 7, 4, 1, 2, 5, 3 },
                          { 9, 7, 3, 6, 5, 4, 8, 2, 1 },
                          { 2, 4, 8, 1, 9, 3, 7, 6, 5 },
                          { 6, 1, 5, 8, 7, 2, 3, 9, 4 }
        };

        bool[,] n1_visible = { { false, false, false, false, false, false, true, true, false },
                          { true, false, false, true, false, false, true, false, true },
                          { true, false, false, true, true, true, false, false, false },
                          { false, true, false, false, false, false, true, false, false },
                          { false, true, false, true, false, true, false, true, false },
                          { false, false, true, false, false, false, false, true, false },
                          { false, false, false, true, true, true, false, false, true },
                          { true, false, true, false, false, true, false, false, true },
                          { false, true, true, false, false, false, false, false, false }

        };






        int[,] n2_answer = { { 4, 9, 8, 6, 2, 5, 3, 7, 1 },
                          { 1, 5, 3, 7, 4, 8, 2, 6, 9 },
                          { 7, 6, 2, 9, 3, 1, 4, 5, 8 },
                          { 9, 3, 4, 8, 1, 7, 5, 2, 6 },
                          { 2, 7, 5, 3, 9, 6, 8, 1, 4 },
                          { 8, 1, 6, 2, 5, 4, 7, 9, 3 },
                          { 6, 4, 1, 5, 7, 3, 9, 8, 2 },
                          { 5, 8, 9, 4, 6, 2, 1, 3, 7 },
                          { 3, 2, 7, 1, 8, 9, 6, 4, 5 }
        };

        bool[,] n2_visible = { { false, false, false, true, true, false, true, false, true },
                          { false, false, false, false, false, true, true, true, false },
                          { false, false, true, false, false, false, false, false, true },
                          { false, true, true, false, true, false, true, true, false },
                          { false, false, false, false, true, false, false, false, false },
                          { false, true, true, false, true, false, true, true, false },
                          { true, false, false, false, false, false, true, false, false },
                          { false, true, true, true, false, false, false, false, false },
                          { true, false, true, false, true, true, false, false, false }

        };



        int[,] n3_answer = { { 5, 9, 1, 3, 2, 8, 7, 4, 6 },
                          { 6, 7, 4, 9, 5, 1, 8, 2, 3 },
                          { 2, 3, 8, 6, 7, 4, 1, 9, 5 },
                          { 3, 5, 6, 2, 9, 7, 4, 8, 1 },
                          { 4, 2, 7, 1, 8, 5, 3, 6, 9 },
                          { 8, 1, 9, 4, 6, 3, 2, 5, 7 },
                          { 9, 8, 5, 7, 3, 2, 6, 1, 4 },
                          { 7, 4, 2, 5, 1, 6, 9, 3, 8 },
                          { 1, 6, 3, 8, 4, 9, 5, 7, 2 }
        };

        bool[,] n3_visible = { { true, false, false, true, false, true, false, false, true },
                          { false, false, true, false, true, false, true, false, false },
                          { true, false, true, false, false, false, true, false, true },
                          { false, false, true, true, false, true, true, false, false },
                          { false, false, false, false, false, false, false, false, false },
                          { false, false, true, true, false, true, true, false, false },
                          { true, false, true, false, false, false, true, false, true },
                          { false, false, true, false, true, false, true, false, false },
                          { true, false, false, true, false, true, false, false, true }

        };


        int[,] n4_answer = { { 3, 9, 6, 8, 1, 2, 7, 5, 4 },
                          { 8, 2, 5, 7, 9, 4, 6, 1, 3 },
                          { 4, 1, 7, 3, 5, 6, 8, 2, 9 },
                          { 5, 8, 3, 2, 4, 7, 1, 9, 6 },
                          { 7, 4, 2, 1, 9, 6, 5, 3, 8 },
                          { 1, 6, 9, 5, 8, 3, 4, 7, 2 },
                          { 6, 7, 8, 9, 2, 5, 3, 4, 1 },
                          { 2, 5, 1, 4, 3, 8, 9, 6, 7 },
                          { 9, 3, 4, 6, 7, 1, 2, 8, 5 }
        };

        bool[,] n4_visible = { { false, true, false, false, true, true, false, false, false },
                          { false, true, true, true, false, false, false, false, false },
                          { true, true, true, false, false, true, false, false, false },
                          { false, false, false, false, false, true, false, true, false },
                          { false, true, true, false, false, false, true, true, false },
                          { false, true, false, true, false, false, false, false, false },
                          { false, false, false, true, false, false, true, true, true },
                          { false, false, false, false, false, true, true, true, false },
                          { false, false, false, true, true, false, false, true, false }

        };






        int[,] n5_answer = { { 4, 6, 8, 1, 3, 7, 9, 2, 5 },
                          { 1, 9, 7, 6, 2, 5, 4, 3, 8 },
                          { 3, 5, 2, 4, 8, 9, 1, 7, 6 },
                          { 8, 1, 9, 7, 5, 4, 3, 6, 2 },
                          { 6, 2, 3, 8, 9, 1, 5, 4, 7 },
                          { 5, 7, 4, 2, 6, 3, 8, 1, 9 },
                          { 7, 8, 5, 3, 4, 6, 2, 9, 1 },
                          { 9, 3, 1, 5, 7, 2, 6, 8, 4 },
                          { 2, 4, 6, 9, 1, 8, 7, 5, 3 }
        };

        bool[,] n5_visible = { { false, true, false, true, false, true, false, true, false },
                          { false, true, true, false, false, false, true, true, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, true, true, true, true, true, false, false },
                          { false, false, false, false, false, false, false, false, false},
                          { false, false, true, true, true, true, true, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, true, true, false, false, false, true, true, false },
                          { false, true, false, true, false, true, false, true, false }

        };




        int[,] h1_answer = { { 5, 7, 6, 9, 4, 8, 1, 3, 2 },
                          { 9, 1, 4, 7, 3, 2, 6, 5, 8 },
                          { 2, 3, 8, 5, 6, 1, 7, 9, 4 },
                          { 3, 8, 5, 2, 1, 4, 9, 6, 7 },
                          { 4, 9, 1, 3, 7, 6, 2, 8, 5 },
                          { 7, 6, 2, 8, 5, 9, 3, 4, 1 },
                          { 6, 2, 3, 4, 8, 7, 5, 1, 9 },
                          { 1, 4, 9, 6, 2, 5, 8, 7, 3 },
                          { 8, 5, 7, 1, 9, 3, 4, 2, 6 }
        };

        bool[,] h1_visible = { { true, false, false, false, false, true, true, false, false },
                          { true, true, false, true, true, false, false, false, false },
                          { false, false, true, false, false, false, false, true, false },
                          { true, false, false, false, false, false, true, true, false },
                          { true, true, false, false, false, false, false, true, true },
                          { false, true, true, false, false, false, false, false, true },
                          { false, true, false, false, false, false, true, false, false },
                          { false, false, false, false, true, true, false, true, true },
                          { false, false, true, true, false, false, false, false, true }

        };



        int[,] h2_answer = { { 5, 1, 3, 8, 9, 6, 4, 2, 7 },
                          { 7, 4, 2, 3, 1, 5, 6, 9, 8 },
                          { 9, 6, 8, 7, 4, 2, 5, 3, 1 },
                          { 1, 5, 4, 2, 8, 9, 3, 7, 6 },
                          { 3, 7, 6, 1, 5, 4, 2, 8, 9 },
                          { 2, 8, 9, 6, 3, 7, 1, 5, 4 },
                          { 8, 9, 1, 5, 6, 3, 7, 4, 2 },
                          { 4, 2, 5, 9, 7, 1, 8, 6, 3 },
                          { 6, 3, 7, 4, 2, 8, 9, 1, 5 }
        };

        bool[,] h2_visible = { { false, false, false, true, true, true, false, false, true },
                          { false, false, true, true, false, true, false, false, false },
                          { false, false, true, false, false, false, true, false, true },
                          { true, true, false, false, false, true, false, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, false, true, false, false, false, true, true },
                          { true, false, true, false, false, false, true, false, false },
                          { false, false, false, true, false, true, true, false, false },
                          { true, false, false, true, true, true, false, false, false }

        };


        int[,] h3_answer = { { 8, 6, 1, 5, 4, 2, 7, 9, 3 },
                          { 9, 4, 3, 1, 7, 8, 5, 6, 2 },
                          { 2, 5, 7, 3, 6, 9, 4, 1, 8 },
                          { 3, 2, 4, 6, 9, 5, 8, 7, 1 },
                          { 6, 8, 9, 7, 1, 4, 3, 2, 5 },
                          { 7, 1, 5, 8, 2, 3, 6, 4, 9 },
                          { 1, 9, 8, 4, 5, 7, 2, 3, 6 },
                          { 5, 7, 2, 9, 3, 6, 1, 8, 4 },
                          { 4, 3, 6, 2, 8, 1, 9, 5, 7 }
        };

        bool[,] h3_visible = { { false, false, false, false, true, false, false, false, false },
                          { true, false, true, false, false, false, false, false, false },
                          { false, true, true, false, true, false, true, false, true },
                          { true, false, false, true, false, false, false, true, false },
                          { true, false, false, true, false, true, false, false, true },
                          { false, true, false, false, false, true, false, false, true },
                          { true, false, true, false, true, false, true, true, false },
                          { false, false, false, false, false, false, true, false, true },
                          { false, false, false, false, true, false, false, false, false }

        };


        int[,] h4_answer = { { 3, 9, 7, 4, 8, 5, 1, 6, 2 },
                          { 2, 5, 1, 6, 9, 7, 3, 4, 8 },
                          { 8, 4, 6, 2, 3, 1, 9, 7, 5 },
                          { 6, 2, 4, 1, 5, 9, 8, 3, 7 },
                          { 7, 8, 9, 3, 2, 4, 6, 5, 1 },
                          { 1, 3, 5, 8, 7, 6, 2, 9, 4 },
                          { 9, 6, 8, 5, 4, 2, 7, 1, 3 },
                          { 5, 7, 2, 9, 1, 3, 4, 8, 6 },
                          { 4, 1, 3, 7, 6, 8, 5, 2, 9 }
        };

        bool[,] h4_visible = { { false, true, true, true, false, true, false, false, false },
                          { true, false, false, true, false, false, false, false, false },
                          { false, false, true, false, true, true, true, false, false },
                          { false, false, false, false, false, true, true, false, true },
                          { true, false, false, false, false, false, false, false, true },
                          { true, false, true, true, false, false, false, false, false },
                          { false, false, true, true, true, false, true, false, false },
                          { false, false, false, false, false, true, false, false, true },
                          { false, false, false, true, false, true, true, true, false }

        };



        int[,] h5_answer = { { 1, 5, 4, 8, 7, 3, 2, 9, 6 },
                          { 7, 9, 2, 5, 1, 6, 4, 8, 3 },
                          { 6, 3, 8, 4, 2, 9, 1, 7, 5 },
                          { 3, 7, 9, 6, 8, 4, 5, 2, 1 },
                          { 2, 6, 5, 7, 3, 1, 9, 4, 8 },
                          { 4, 8, 1, 9, 5, 2, 6, 3, 7 },
                          { 9, 1, 6, 3, 4, 7, 8, 5, 2 },
                          { 8, 4, 7, 2, 6, 5, 3, 1, 9 },
                          { 5, 2, 3, 1, 9, 8, 7, 6, 4 }
        };

        bool[,] h5_visible = { { false, true, true, false, true, false, false, false, false },
                          { false, false, false, false, false, false, true, false, true },
                          { true, true, false, false, true, true, false, false, false },
                          { true, false, false, true, false, true, false, false, true },
                          { false, false, true, false, false, false, true, false, false },
                          { true, false, false, true, false, true, false, false, true },
                          { false, false, false, true, true, false, false, true, true },
                          { true, false, true, false, false, false, false, false, false },
                          { false, false, false, false, true, false, true, true, false }

        };






        int[,] x1_answer = { { 1, 2, 5, 6, 4, 9, 7, 8, 3 },
                          { 9, 3, 8, 5, 7, 1, 4, 2, 6 },
                          { 7, 6, 4, 3, 2, 8, 9, 1, 5 },
                          { 5, 9, 6, 8, 3, 7, 1, 4, 2 },
                          { 4, 7, 2, 1, 6, 5, 8, 3, 9 },
                          { 8, 1, 3, 4, 9, 2, 5, 6, 7 },
                          { 3, 8, 1, 9, 5, 6, 2, 7, 4 },
                          { 6, 5, 7, 2, 8, 4, 3, 9, 1 },
                          { 2, 4, 9, 7, 1, 3, 6, 5, 8 }
        };

        bool[,] x1_visible = { { false, false, true, false, false, false, true, true, false },
                          { true, false, false, false, true, true, false, false, false },
                          { false, false, true, true, false, false, true, false, false },
                          { false, false, false, false, true, false, true, false, true },
                          { false, false, true, true, false, true, true, false, false },
                          { true, false, true, false, true, false, false, false, false },
                          { false, false, true, false, false, true, true, false, false },
                          { false, false, false, true, true, false, false, false, true },
                          { false, true, true, false, false, false, true, false, false }

        };



        int[,] x2_answer = { { 7, 3, 4, 5, 9, 8, 1, 6, 2 },
                          { 2, 1, 8, 4, 7, 6, 3, 9, 5 },
                          { 5, 6, 9, 2, 3, 1, 4, 7, 8 },
                          { 9, 2, 5, 8, 4, 3, 7, 1, 6 },
                          { 3, 7, 1, 9, 6, 2, 5, 8, 4 },
                          { 4, 8, 6, 7, 1, 5, 9, 2, 3 },
                          { 1, 5, 3, 6, 8, 7, 2, 4, 9 },
                          { 6, 4, 2, 1, 5, 9, 8, 3, 7 },
                          { 8, 9, 7, 3, 2, 4, 6, 5, 1 }
        };

        bool[,] x2_visible = { { false, true, false, true, false, true, false, false, true },
                          { false, false, true, false, false, false, false, false, false },
                          { true, false, false, false, false, true, true, false, false },
                          { true, false, false, false, false, true, true, true, true },
                          { false, false, false, false, false, false, false, false, false },
                          { true, true, true, true, false, false, false, false, true },
                          { false, false, true, true, false, false, false, false, true },
                          { false, false, false, false, false, false, true, false, false },
                          { true, false, false, true, false, true, false, true, false }

        };

        int[,] x3_answer = { { 6, 3, 5, 4, 9, 2, 1, 8, 7 },
                          { 9, 2, 8, 7, 6, 1, 5, 4, 3 },
                          { 1, 7, 4, 8, 5, 3, 6, 9, 2 },
                          { 5, 4, 6, 2, 8, 7, 9, 3, 1 },
                          { 2, 1, 9, 5, 3, 4, 7, 6, 8 },
                          { 3, 8, 7, 9, 1, 6, 4, 2, 5 },
                          { 8, 5, 2, 6, 7, 9, 3, 1, 4 },
                          { 4, 9, 3, 1, 2, 5, 8, 7, 6 },
                          { 7, 6, 1, 3, 4, 8, 2, 5, 9 }
        };

        bool[,] x3_visible = { { false, false, true, true, false, true, true, false, false },
                          { false, true, false, false, true, false, false, true, false },
                          { true, true, false, false, false, false, false, true, true },
                          { false, false, false, true, false, true, false, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, false, true, false, true, false, false, false },
                          { true, true, false, false, false, false, false, true, true },
                          { false, true, false, false, true, false, false, true, false },
                          { false, false, true, true, false, true, true, false, false }

        };



        int[,] x4_answer = { { 8, 6, 1, 4, 9, 7, 2, 5, 3 },
                          { 3, 5, 2, 8, 1, 6, 4, 7, 9 },
                          { 9, 4, 7, 3, 2, 5, 6, 1, 8 },
                          { 6, 3, 4, 1, 5, 9, 8, 2, 7 },
                          { 7, 2, 9, 6, 8, 4, 5, 3, 1 },
                          { 5, 1, 8, 2, 7, 3, 9, 6, 4 },
                          { 4, 9, 5, 7, 3, 2, 1, 8, 6 },
                          { 1, 7, 6, 5, 4, 8, 3, 9, 2 },
                          { 2, 8, 3, 9, 6, 1, 7, 4, 5 }
        };

        bool[,] x4_visible = { { true, true, false, true, false, false, true, true, false },
                          { false, true, false, true, false, false, false, false, true },
                          { true, false, false, false, true, false, false, false, false },
                          { false, true, true, false, false, false, true, false, false },
                          { true, false, false, false, false, false, false, false, true },
                          { false, false, true, false, false, false, true, true, false },
                          { false, false, false, false, true, false, false, false, true },
                          { true, false, false, false, false, true, false, true, false },
                          { false, true, true, false, false, true, false, true, true }

        };




        int[,] x5_answer = { { 5, 3, 2, 4, 8, 6, 1, 7, 9 },
                          { 7, 6, 9, 1, 2, 3, 5, 8, 4 },
                          { 4, 8, 1, 5, 7, 9, 3, 6, 2 },
                          { 9, 4, 3, 7, 6, 2, 8, 1, 5 },
                          { 8, 1, 7, 9, 4, 5, 2, 3, 6 },
                          { 6, 2, 5, 3, 1, 8, 9, 4, 7 },
                          { 2, 5, 4, 8, 3, 7, 6, 9, 1 },
                          { 3, 7, 6, 2, 9, 1, 4, 5, 8 },
                          { 1, 9, 8, 6, 5, 4, 7, 2, 3 }
        };

        bool[,] x5_visible = { { false, true, false, false, false, true, true, true, false },
                          { false, true, true, false, false, false, false, false, true },
                          { true, false, false, false, true, false, false, false, true },
                          { false, false, true, true, true, false, false, false, false },
                          { false, false, false, true, false, true, false, false, false },
                          { false, false, false, false, true, true, true, false, false },
                          { true, false, false, false, true, false, false, false, true },
                          { true, false, false, false, false, false, true, true, false },
                          { false, true, true, true, false, false, false, true, false }

        };
    }
}
