

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

using System.Drawing;
namespace WarApp{

  enum suites {clubs,diamonds,hearts,spades};

  class Deck{

      private List< Tuple<int,int>> cards = new List<  Tuple <int,int> >();// a list of the cards in the deck
      public int DeckSize = 0;
      public Deck(string start = "full"){
        if(start == "full"){// checks if the user wants a full deck, otherwise an empty one is provided
          for(int i = 0;i<13; i++){
            for(int j=0;j<4;j++){
              cards.Add(Tuple.Create(i,j));
            }
          }// adds all the cards into the deck
          DeckSize = cards.Count;
          shuffle();// shuffle the deck b4 use
          }
        else if(start=="empty"){
          cards.Clear();
        }

        }


      public void shuffle(){// puts the cards in a random order using the Fisher-Yates shuffle
          Random rng = new Random();
          int n = DeckSize;
          while(n>1){
            n--;
            int k = rng.Next(n+1);
            var value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
          }


      }
      public void AddCard(Tuple <int,int> card){// just a member function to add cards
        cards.Add(card);
        DeckSize++;
      }
      public void AddCardToBottom(Tuple <int,int> C){// method to add cards to the bottom of the deck
          cards.Insert(0,C);
          DeckSize++;
      }

      public bool isEmpty(){// method for checking if deck is empty
        if (DeckSize>0){
          return false;
        }
        else{
          Console.WriteLine("Deck is empty()");
          return true;
        }
      }
      public Tuple<int,int> mill(){ // remove the top card card and returns it to you
        var value= new Tuple<int,int>(-1,-1);
        if(!isEmpty()){ //checks if there any cards in the deck
          DeckSize--; //shrinks size of deck
          value = cards[DeckSize]; // gets the last elements
          cards.RemoveAt(DeckSize); // removes
        }

        return value;
      }

      public Tuple<int,int> showTop(){// shows the top of the deck without removing it
        return cards[DeckSize-1];
      }

      public void showDeck(){ // prints out the entire deck in order
        Console.WriteLine("Current Deck:");
        for(int i=0;i<DeckSize;i++){
          Console.WriteLine("the {0}th card is the {1} of {2}",i+1,cards[i].Item1,(suites)cards[i].Item2);
        }
      }
      public Deck split(){// splits the deck in two and returns on half. The other is stored in the original.
        Deck D = new Deck("empty");
        int otherDeckSize = DeckSize/2;
        while(DeckSize>otherDeckSize){// keeps going till half the list is empty
          D.AddCard(mill());// adds the cards to the other deck
        }
        return D;
      }

      public void combine(Deck D){// takes anouther deck an combines it
        for(int i=0;i<D.DeckSize;i++){
          AddCard(D.mill());
          DeckSize++;
        }
      }

  }
  class War: Form  {
    Label RulesLabel;
    PictureBox DeckImage1,DeckImage2,FaceUp1,FaceUp2;

    Button StartButton;
    Label Player1DeckSize;
    Label Player2DeckSize;
    Button ActionButton;
    List <Tuple <int,int>> Player1Stack;
    List <Tuple <int,int>> Player2Stack;
    Deck D1,D2;
    public void InitRulesLabel(){
      RulesLabel = new Label();
      RulesLabel.AutoSize = false;
      RulesLabel.BorderStyle = BorderStyle.FixedSingle ;
      RulesLabel.UseMnemonic = true;
      RulesLabel.Font = new Font(RulesLabel.Font.FontFamily, 16);
      RulesLabel.Text =   @"Rules:
      Each player starts out with a a pile of 26 cards.
      During each turn both players reveal the top card of their deck.
      The player who revealed the card with the highest rank get to put
      both on the bottom of their pile.If there is a tie, each player
      reveals a face up and face down card.The player with the highest
      rank from this pair gets all six cards.If the cards are still tied,
      then continue having wars until the cards are no longered tied.
      Once a player runs out of cards they lose.
      So don't run out of cards :) ";
      RulesLabel.Size = new Size (RulesLabel.PreferredWidth, RulesLabel.PreferredHeight);
      RulesLabel.TextAlign = ContentAlignment.MiddleLeft;
      //RulesLabel.LineSpacing =360;
      //RulesLabel.Location = new Point(0, 0);
      RulesLabel.BackColor = Color.White;
      RulesLabel.ForeColor = Color.Black ;
      RulesLabel.BorderStyle = BorderStyle.FixedSingle;
      RulesLabel.Size = new Size(RulesLabel.PreferredWidth, RulesLabel.PreferredHeight + 2);
    }
    public void InitDeckImage(ref PictureBox L){
      //L = new PictureBox();
      Image im = Image.FromFile("cardimages/red_back.png");

      L.Size = new Size(130, 200);
      L.Image = (Image)(new Bitmap(im, L.Size));

    }
    void InitCardImage(ref PictureBox L,int r,int s){
      string[] ranks ={"2","3","4","5","6","7","8","9","10","J","Q","K","A"};
      string[] suites = {"C","D","H","S"};
      //L = new PictureBox();
      Image im = Image.FromFile("cardimages/"+ranks[r]+suites[s]+".png");

      L.Size = new Size(130, 200);
      L.Image = (Image)(new Bitmap(im, L.Size));
    }
    public void InitializeComponents(){
      DeckImage1 = new PictureBox();
      InitDeckImage(ref DeckImage1);
      InitRulesLabel();
      InitStartButton();

      DeckImage1.Location = new Point(0,0);
      RulesLabel.Location = new Point(0, 300);
      this.Controls.Add(RulesLabel);
      this.Controls.Add(DeckImage1);
      this.Controls.Add(StartButton);

    }
    public void InitStartButton(){
      StartButton = new Button();
      StartButton.Text = "Press to Begin!";
      StartButton.ForeColor = Color.Black;
      StartButton.BackColor = Color.White;
      StartButton.Location = new Point(200,150);
      StartButton.Font = new Font(StartButton.Font.FontFamily, 20);
      StartButton.Size = new Size (200, 75);
      StartButton.Click+= StartClick;
    }
    void StartClick(object sender, EventArgs e)
    {
        MessageBox.Show("Let's get to it!");

        this.Controls.Clear();
        StartGame();
    }

    void InitBoard(){

      Player1DeckSize.Text = "Deck Size: "+D1.DeckSize;
      Player2DeckSize.Text = "Deck Size: "+D2.DeckSize;
      Player1DeckSize.Location = new Point(0,200);
      Player2DeckSize.Location = new Point(550,200);
      Player1DeckSize.Font = new Font(Player1DeckSize.Font.FontFamily, 12);
      Player2DeckSize.Font = new Font(Player1DeckSize.Font.FontFamily, 12);
      Player1DeckSize.Size = new Size (120, 40);
      Player2DeckSize.Size = new Size (120, 40);
      Player1DeckSize.ForeColor = Color.Black;
      Player1DeckSize.BackColor = Color.White;
      Player2DeckSize.ForeColor = Color.Black;
      Player2DeckSize.BackColor = Color.White;


      ActionButton.Size = new Size (200, 125);
      ActionButton.Font = new Font(ActionButton.Font.FontFamily, 20);
      ActionButton.ForeColor = Color.Black;
      ActionButton.BackColor = Color.White;
      ActionButton.Text = "Make a Move";

      InitDeckImage(ref DeckImage1);
      InitDeckImage(ref DeckImage2);
      DeckImage1.Location =  new Point(0,0);

      ActionButton.Location = new Point(275,75);

      DeckImage2.Location =  new Point(550,0);
      this.Controls.Add(FaceUp1);
      this.Controls.Add(FaceUp2);



      this.Controls.Add(DeckImage1);
      this.Controls.Add(DeckImage2);
      this.Controls.Add(DeckImage1);
      this.Controls.Add(Player1DeckSize);
      this.Controls.Add(Player2DeckSize);

      this.Controls.Add(ActionButton);

      ActionButton.Click += ActionClick;

    }
    void EndClick(object sender, EventArgs e){
      this.Close();
    }
    void ActionClick(object sender, EventArgs e)
    {





        if(Player1Stack.Count==0){
          Player1Stack.Add(D1.mill());
          Player2Stack.Add(D2.mill());
        }


        else if(Player1Stack[Player1Stack.Count-1].Item1 == Player2Stack[Player2Stack.Count-1].Item1){
          MessageBox.Show("Time For War!");
          Player1Stack.Add(D1.mill());
          Player2Stack.Add(D2.mill());
          Player1Stack.Add(D1.mill());
          Player2Stack.Add(D2.mill());

          Player1DeckSize.Text = "Deck Size: "+D1.DeckSize;
          Player2DeckSize.Text = "Deck Size: "+D2.DeckSize;
        }
        else if(Player1Stack[Player1Stack.Count-1].Item1 > Player2Stack[Player2Stack.Count-1].Item1){

          MessageBox.Show("Player 1 wins the round!");
          for(int i = 0; i<Player1Stack.Count;i++){
            D1.AddCardToBottom(Player1Stack[i]);
          }
          for(int i = 0; i<Player2Stack.Count;i++){
            D1.AddCardToBottom(Player2Stack[i]);
          }
          Player1Stack.Clear();
          Player2Stack.Clear();

          Player1DeckSize.Text = "Deck Size: "+D1.DeckSize;
          Player2DeckSize.Text = "Deck Size: "+D2.DeckSize;

        }
        else if(Player1Stack[Player1Stack.Count-1].Item1 < Player2Stack[Player2Stack.Count-1].Item1){

          MessageBox.Show("Player 2 wins the round!");
          for(int i = 0; i<Player1Stack.Count;i++){
            D2.AddCardToBottom(Player1Stack[i]);
          }
          for(int i = 0; i<Player2Stack.Count;i++){
            D2.AddCardToBottom(Player2Stack[i]);
          }
          Player1Stack.Clear();
          Player2Stack.Clear();

          Player1DeckSize.Text = "Deck Size: "+D1.DeckSize;
          Player2DeckSize.Text = "Deck Size: "+D2.DeckSize;

        }
        if(D1.DeckSize==0){
          MessageBox.Show("Player 1 won!");
          ActionButton.Text = "Play Again?";
          ActionButton.Click += EndClick;
          return;
        }
        else if(D2.DeckSize==0){
          MessageBox.Show("Player 2 won!");
          ActionButton.Text = "Play Again?";
          ActionButton.Click += EndClick;
          return;
        }
        if(Player1Stack.Count!=0){
        this.Controls.Remove(FaceUp1);
        this.Controls.Remove(FaceUp2);
        FaceUp1 = new PictureBox();
        FaceUp2 = new PictureBox();
        FaceUp1.Location =  new Point(150,250);
        FaceUp2.Location =  new Point(450,250);
        InitCardImage(ref FaceUp1,Player1Stack[Player1Stack.Count-1].Item1,Player1Stack[Player1Stack.Count-1].Item2);
        InitCardImage(ref FaceUp2,Player2Stack[Player2Stack.Count-1].Item1,Player2Stack[Player2Stack.Count-1].Item2);
        this.Controls.Add(FaceUp1);
        this.Controls.Add(FaceUp2);

        }
        else{
          this.Controls.Remove(FaceUp1);
          this.Controls.Remove(FaceUp2);


        }
        Player1DeckSize.Text = "Deck Size: "+D1.DeckSize;
        Player2DeckSize.Text = "Deck Size: "+D2.DeckSize;


    }

    public void StartGame(){

      D1 = new Deck();
      D2 = D1.split();
      D2 = D2.split().split().split();
      DeckImage1 = new PictureBox();
      DeckImage2 = new PictureBox();

      ActionButton = new Button();

      Player1DeckSize = new Label();
      Player2DeckSize = new Label();



      Player1Stack = new List<Tuple<int,int>>();
      Player2Stack = new List<Tuple<int,int>>();

      //D1.showDeck();
      InitBoard();





    }
    public War()
    {
      InitializeComponents();
      this.Text = "War!";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(700, 700); //Size except the Title Bar-CaptionHeight
      this.MaximizeBox = false;
      this.BackColor = Color.Green;


    }

    public static void Main() {

          Application.Run(new War());

         }

  }

}
